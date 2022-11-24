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
  /// Get difference in quote currency between each new and current <see cref="Allocation"/> in respectively <paramref name="newBalance"/> and <paramref name="curBalance"/>.
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

  public static IOrder ConstructSellOrder(this IExchangeService @this, KeyValuePair<Allocation, decimal> quoteDiff)
  {
    bool terminatePosition =
      quoteDiff.Key.AmountQuote - quoteDiff.Value <= @this.MinimumOrderSize;

    // Prevent dust.
    OrderArgs orderArgs = !terminatePosition
      ? new()
      {
        AmountQuote = quoteDiff.Value,
      }
      : new()
      {
        Amount = quoteDiff.Key.Amount,
      };

    // Expected fee.
    decimal feeExpected = !terminatePosition
      ? @this.TakerFee * quoteDiff.Value
      : @this.TakerFee * quoteDiff.Key.AmountQuote;

    // Sell order ..
    return new Order(
      quoteDiff.Key.Market,
      Abstracts.Enums.OrderSide.Sell,
      Abstracts.Enums.OrderType.Market,
      orderArgs)
    {
      FeeExpected = feeExpected
    };
  }

  /// <summary>
  /// Sell pieces of oversized <see cref="Allocation"/>s in order for those to meet <paramref name="newBalance"/>.
  /// Completes when verified that all triggered sell orders are ended.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newBalance"></param>
  /// <param name="curBalance"></param>
  /// <returns></returns>
  public static async Task<IOrder[]> SellOveragesAndVerify(this IExchangeService @this, Balance newBalance, Balance? curBalance = null)
  {
    // Fetch balance if not provided.
    curBalance ??= await @this.GetBalance();

    // Get enumerable since we're iterating it just once.
    IEnumerable<KeyValuePair<Allocation, decimal>> quoteDiffs = GetAllocationQuoteDiffs(newBalance, curBalance);

    var sellTasks = new List<Task<IOrder>>();

    // The sell task loop ..
    foreach (KeyValuePair<Allocation, decimal> quoteDiff in quoteDiffs)
    {
      if (quoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency))
      {
        // We can't sell quote currency for quote currency.
        continue;
      }

      // Positive quote differences refer to oversized allocations.
      if (quoteDiff.Value >= @this.MinimumOrderSize)
      {
        // Prevent dust.
        OrderArgs orderArgs =
          quoteDiff.Key.AmountQuote - quoteDiff.Value > @this.MinimumOrderSize
          ? new()
          {
            AmountQuote = quoteDiff.Value,
          }
          : new()
          {
            Amount = quoteDiff.Key.Amount,
          };

        // Sell ..
        sellTasks.Add(@this.NewOrder(
          quoteDiff.Key.Market,
          Abstracts.Enums.OrderSide.Sell,
          Abstracts.Enums.OrderType.Market,
          orderArgs)
          // Continue to verify sell order ended, within same task to optimize performance.
          .ContinueWith(sellTask => @this.VerifyOrderEnded(sellTask.Result)).Unwrap());
      }
    }

    return await Task.WhenAll(sellTasks);
  }

  /// <summary>
  /// Buy to increase undersized <see cref="Allocation"/>s in order for those to meet <paramref name="newBalance"/>.
  /// <see cref="Allocation"/> differences are scaled relative to available quote currency.
  /// Completes when orders are placed.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newBalance"></param>
  /// <returns></returns>
  public static async Task<IOrder[]> BuyUnderages(this IExchangeService @this, Balance newBalance)
  {
    // Force fetch current balance.
    Balance curBalance = await @this.GetBalance();

    // Load in memory using .ToList(), to avoid re-enumerating since we're iterating it more than once.
    // IS THIS REALLY THE PREFERED WAY ? !!
    var quoteDiffs = GetAllocationQuoteDiffs(newBalance, curBalance).ToList();

    // Absolute sum of all negative quote differences except of quote currency.
    decimal totalBuy = Math.Abs(quoteDiffs
      .FindAll(quoteDiff =>
        !quoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency)
        && quoteDiff.Value < 0)
      .Sum(quoteDiff => quoteDiff.Value));

    // Multiplication ratio to avoid potentially oversized buy order sizes.
    decimal ratio = Math.Min(totalBuy, curBalance.AmountQuoteAvailable) / totalBuy;

    var buyTasks = new List<Task<IOrder>>();

    // The buy task loop ..
    foreach (KeyValuePair<Allocation, decimal> quoteDiff in quoteDiffs)
    {
      if (quoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency))
      {
        // We can't buy quote currency with quote currency.
        continue;
      }

      // Scale to avoid potentially oversized buy order sizes.
      // First check eligibility as it is less expensive operation than the multiplication operation.
      decimal amountQuote =
        quoteDiff.Value <= -@this.MinimumOrderSize ? ratio * quoteDiff.Value : 0;

      // Negative quote differences refer to undersized allocations.
      if (amountQuote <= -@this.MinimumOrderSize)
      {
        // Buy ..
        buyTasks.Add(@this.NewOrder(
          quoteDiff.Key.Market,
          Abstracts.Enums.OrderSide.Buy,
          Abstracts.Enums.OrderType.Market,
          new OrderArgs
          {
            AmountQuote = amountQuote,
          }));
      }
    }

    return await Task.WhenAll(buyTasks);
  }

  public static IEnumerable<IOrder> EstimateFee(this IExchangeService @this, Balance newBalance, Balance curBalance)
  {
    // Get enumerable since we're iterating it just once.
    IEnumerable<KeyValuePair<Allocation, decimal>> quoteDiffs = GetAllocationQuoteDiffs(newBalance, curBalance);

    // The order loop ..
    foreach (KeyValuePair<Allocation, decimal> quoteDiff in quoteDiffs)
    {
      if (quoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency))
      {
        // We can't sell quote currency for quote currency
        // and we can't buy quote currency with quote currency.
        continue;
      }

      // Positive quote differences refer to oversized allocations.
      if (quoteDiff.Value >= @this.MinimumOrderSize)
      {
        // Sell order ..
        yield return @this.ConstructSellOrder(quoteDiff);
      }
      // Negative quote differences refer to undersized allocations.
      else if (quoteDiff.Value <= -@this.MinimumOrderSize)
      {
        // Buy order ..
        yield return new Order(
          quoteDiff.Key.Market,
          Abstracts.Enums.OrderSide.Sell,
          Abstracts.Enums.OrderType.Market,
          new OrderArgs
          {
            AmountQuote = quoteDiff.Value,
          })
        {
          FeeExpected = quoteDiff.Value * @this.TakerFee,
        };
      }
    }
  }

  /// <summary>
  /// Asynchronously performs a portfolio rebalance.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newBalance"></param>
  public static async Task<IEnumerable<IOrder>> Rebalance(this IExchangeService @this, Balance newBalance)
  {
    // Clear the path ..
    await @this.CancelAllOpenOrders();

    // Sell pieces of oversized allocations first,
    // so we have sufficient quote currency available to buy with.
    IOrder[] sellResults = await @this.SellOveragesAndVerify(newBalance);

    // Then buy to increase undersized allocations.
    IOrder[] buyResults = await @this.BuyUnderages(newBalance);

    return sellResults.Concat(buyResults);
  }
}