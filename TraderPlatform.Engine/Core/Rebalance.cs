using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;

namespace TraderPlatform.Engine.Core;

public static partial class Trader
{
  /// <summary>
  /// Get difference in quote currency between <paramref name="newAlloc"/> and <paramref name="curAlloc"/>.
  /// </summary>
  /// <param name="newAlloc"></param>
  /// <param name="curAlloc"></param>
  /// <returns><paramref name="curAlloc"/> and its difference in quote currency compared to given <paramref name="newAlloc"/>.</returns>
  public static KeyValuePair<Allocation, decimal> GetAllocationQuoteDiff(Allocation newAlloc, Allocation curAlloc)
  {
    return new KeyValuePair<Allocation, decimal>(curAlloc, curAlloc.AmountQuote - newAlloc.AmountQuote);
  }

  /// <summary>
  /// Get difference in quote currency between each new and current <see cref="Allocation"/> in <paramref name="newBalance"/> and <paramref name="curBalance"/>.
  /// </summary>
  /// <param name="newBalance"></param>
  /// <param name="curBalance"></param>
  /// <returns>Collection of current <see cref="Allocation"/>s and their differences in quote currency compared to the new <see cref="Allocation"/>s.</returns>
  public static IEnumerable<KeyValuePair<Allocation, decimal>> GetAllocationQuoteDiffs(Balance newBalance, Balance curBalance)
  {
    foreach (Allocation curAlloc in curBalance.Allocations)
    {
      Allocation newAlloc = newBalance.GetAllocation(curAlloc.Market.BaseCurrency) ?? new(curAlloc.Market, curAlloc.Price, 0);

      yield return GetAllocationQuoteDiff(newAlloc, curAlloc);
    }

    foreach (Allocation newAlloc in newBalance.Allocations)
    {
      Allocation? curAlloc = curBalance.GetAllocation(newAlloc.Market.BaseCurrency);

      if (curAlloc != null)
      {
        // Already covered in previous foreach.
        continue;
      }
      else
      {
        curAlloc = new(newAlloc.Market, newAlloc.Price, 0);
      }

      yield return GetAllocationQuoteDiff(curAlloc, newAlloc);
    }
  }

  /// <summary>
  /// A task that will complete when verified that the given <paramref name="order"/> is ended.
  /// If the given order is not completed within given amount of <paramref name="checks"/>, it will be cancelled.
  /// Every new check is performed one second after the previous has been resolved.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="order"></param>
  /// <param name="checks"></param>
  /// <returns>Completes when verified that the given <paramref name="order"/> is ended.</returns>
  public static async Task<IOrder> VerifyOrderEnded(this IExchangeService @this, IOrder order, int checks = 60)
  {
    while (
      checks > 0 &&
      order.Id != null &&
      !order.Status.HasFlag(
        Abstracts.Enums.OrderStatus.Canceled |
        Abstracts.Enums.OrderStatus.Expired |
        Abstracts.Enums.OrderStatus.Rejected |
        Abstracts.Enums.OrderStatus.Filled))
    {
      await Task.Delay(1000);

      order = await @this.GetOrder(order.Id!, order.Market) ?? order;

      checks--;
    }

    if (checks == 0)
    {
      order = await @this.CancelOrder(order.Id!, order.Market) ?? order;
    }

    return order;
  }

  /// <summary>
  /// Asynchronously performs a portfolio rebalance.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newBalance"></param>
  /// <param name="curBalance"></param>
  public static async Task Rebalance(this IExchangeService @this, Balance newBalance, Balance? curBalance = null)
  {
    curBalance ??= await @this.GetBalance();

    var sellTasks = new List<Task<IOrder>>();

    var quoteDiffs = GetAllocationQuoteDiffs(newBalance, curBalance);

    foreach (KeyValuePair<Allocation, decimal> quoteDiff in quoteDiffs)
    {
      if (quoteDiff.Key.Market.BaseCurrency == @this.QuoteCurrency)
      {
        continue;
      }

      if (quoteDiff.Value > 0)
      {
        // Need to sell since we own too many.
        sellTasks.Add(@this.NewOrder(
          quoteDiff.Key.Market,
          Abstracts.Enums.OrderSide.Sell,
          Abstracts.Enums.OrderType.Market,
          new OrderArgs
          {
            AmountQuote = quoteDiff.Value,
          })
          .ContinueWith(sellTask => @this.VerifyOrderEnded(sellTask.Result)).Unwrap());
      }
      else
      {
        // Need to buy since we own too few.
        // But we need to compensate for spread/slippage first after sell orders are filled.
      }
    }

    // Sell ..
    IOrder[] sellResults = await Task.WhenAll(sellTasks);

    // Fetch balance again in order to compensate for spread/slippage ..
    curBalance = await @this.GetBalance();

    var buyTasks = new List<Task<IOrder>>();

    quoteDiffs = GetAllocationQuoteDiffs(newBalance, curBalance)
      .Where(quoteDiff => quoteDiff.Key.Market.BaseCurrency != @this.QuoteCurrency && quoteDiff.Value < 0);

    decimal totalBuy = quoteDiffs.Sum(quoteDiff => quoteDiff.Value);

    foreach (KeyValuePair<Allocation, decimal> quoteDiff in quoteDiffs)
    {
      // Need to buy since we own too few.
      buyTasks.Add(@this.NewOrder(
        quoteDiff.Key.Market,
        Abstracts.Enums.OrderSide.Buy,
        Abstracts.Enums.OrderType.Market,
        new OrderArgs
        {
          // Compensate for spread/slippage.
          // Dividing negative by negative results in positive.
          AmountQuote = curBalance.AmountQuoteAvailable * quoteDiff.Value / totalBuy,
        }));
    }

    // Buy ..
    IOrder[] buyResults = await Task.WhenAll(buyTasks);
  }
}