using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;

namespace TraderPlatform.Engine.Core;

public static partial class Trader
{
  public static KeyValuePair<Allocation, decimal> GetAllocationQuoteDiff(Allocation newAlloc, Allocation curAlloc)
  {
    return new KeyValuePair<Allocation, decimal>(curAlloc, curAlloc.AmountQuote - newAlloc.AmountQuote);
  }

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

  public static async Task<IOrder> VerifyOrderEnded(this IExchangeService @this, IOrder order)
  {
    int checks = 60;

    while (
      checks > 0 &&
      order.Id != null &&
      !order.Status.HasFlag(
        Abstracts.Enums.OrderStatus.Canceled |
        Abstracts.Enums.OrderStatus.Expired |
        Abstracts.Enums.OrderStatus.Rejected |
        Abstracts.Enums.OrderStatus.Filled))
    {
      Thread.Sleep(1);

      order = await @this.GetOrder(order.Id!, order.Market) ?? order;

      checks--;
    }

    if (checks == 0)
    {
      order = await @this.CancelOrder(order.Id!, order.Market) ?? order;
    }

    return order;
  }

  public static async void Rebalance(this IExchangeService @this, Balance newBalance, Balance? curBalance = null)
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