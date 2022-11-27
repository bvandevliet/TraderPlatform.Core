using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;
using TraderPlatform.Engine.Models;

namespace TraderPlatform.Engine.Core;

public static partial class Trader
{
  /// <summary>
  /// Get current deviation in quote currency when comparing absolute new allocations in
  /// <paramref name="newAssetAllocs"/> against current allocations in <paramref name="curBalance"/>.
  /// </summary>
  /// <param name="newAssetAllocs"></param>
  /// <param name="curBalance"></param>
  /// <returns>Collection of current <see cref="Allocation"/>s and their deviation in quote currency.</returns>
  public static IEnumerable<KeyValuePair<Allocation, decimal>> GetAllocationQuoteDiffs(IEnumerable<AbsAssetAlloc> newAssetAllocs, Balance curBalance)
  {
    // Initialize absolute asset allocation List,
    // being filled using a multi-purpose foreach to eliminate redundant iterations.
    List<AbsAssetAlloc> newAssetAllocsList = new();

    // Sum of all absolute allocation values.
    // being summed up using a multi-purpose foreach to eliminate redundant iterations.
    decimal totalAbsAlloc = 0;

    // Multi-purpose foreach to eliminate redundant iterations.
    foreach (AbsAssetAlloc absAssetAlloc in newAssetAllocs)
    {
      // Add to sum of all absolute allocation values.
      totalAbsAlloc += absAssetAlloc.AbsAlloc;

      // Add to absolute asset allocation List.
      newAssetAllocsList.Add(absAssetAlloc);
    }

    // Loop through current allocations and determine quote diffs.
    foreach (Allocation curAlloc in curBalance.Allocations)
    {
      // Find associated absolute allocation.
      decimal absAlloc =
        newAssetAllocsList.Find(absAssetAlloc => absAssetAlloc.Asset.Equals(curAlloc.Market.BaseCurrency))?.AbsAlloc ?? 0;

      // Determine relative allocation.
      decimal relAlloc = totalAbsAlloc == 0 ? 0 : absAlloc / totalAbsAlloc;

      // Determine new quote amount.
      decimal newAmountQuote = relAlloc * curBalance.AmountQuoteTotal;

      yield return new KeyValuePair<Allocation, decimal>(curAlloc, curAlloc.AmountQuote - newAmountQuote);
    }

    // Loop through absolute asset allocations and determine yet missing quote diffs.
    foreach (AbsAssetAlloc absAssetAlloc in newAssetAllocsList)
    {
      if (null != curBalance.GetAllocation(absAssetAlloc.Asset))
      {
        // Already covered in previous foreach.
        continue;
      }

      // Define current allocation, which is zero here.
      Allocation curAlloc = new(new Market(curBalance.QuoteCurrency, absAssetAlloc.Asset), 0, 0);

      // Determine relative allocation.
      decimal relAlloc = totalAbsAlloc == 0 ? 0 : absAssetAlloc.AbsAlloc / totalAbsAlloc;

      // Determine new quote amount.
      decimal newAmountQuote = relAlloc * curBalance.AmountQuoteTotal;

      yield return new KeyValuePair<Allocation, decimal>(curAlloc, -newAmountQuote);
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
  /// Get a buy order object including the expected fee.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="curAlloc"></param>
  /// <param name="amountQuote"></param>
  /// <returns></returns>
  public static IOrder ConstructBuyOrder(this IExchangeService @this, Allocation curAlloc, decimal amountQuote)
  {
    // Expected fee.
    decimal feeExpected =
      @this.TakerFee * amountQuote;

    return new Order(
      curAlloc.Market,
      Abstracts.Enums.OrderSide.Buy,
      Abstracts.Enums.OrderType.Market,
      new OrderArgs
      {
        AmountQuote = amountQuote,
      })
    {
      FeeExpected = feeExpected,
    };
  }

  /// <summary>
  /// Get a sell order object including the expected fee.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="curAlloc"></param>
  /// <param name="amountQuote"></param>
  /// <returns></returns>
  public static IOrder ConstructSellOrder(this IExchangeService @this, Allocation curAlloc, decimal amountQuote)
  {
    bool terminatePosition =
      curAlloc.AmountQuote - amountQuote <= @this.MinimumOrderSize;

    // Prevent dust.
    OrderArgs orderArgs = !terminatePosition
      ? new()
      {
        AmountQuote = amountQuote,
      }
      : new()
      {
        Amount = curAlloc.Amount,
      };

    // Expected fee.
    decimal feeExpected = !terminatePosition
      ? @this.TakerFee * amountQuote
      : @this.TakerFee * curAlloc.AmountQuote;

    return new Order(
      curAlloc.Market,
      Abstracts.Enums.OrderSide.Sell,
      Abstracts.Enums.OrderType.Market,
      orderArgs)
    {
      FeeExpected = feeExpected,
    };
  }

  /// <summary>
  /// Sell pieces of oversized <see cref="Allocation"/>s as defined in <paramref name="allocQuoteDiffs"/>.
  /// Completes when verified that all triggered sell orders are ended.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="allocQuoteDiffs"></param>
  /// <returns></returns>
  public static async Task<IOrder[]> SellOveragesAndVerify(this IExchangeService @this, IEnumerable<KeyValuePair<Allocation, decimal>> allocQuoteDiffs)
  {
    var sellTasks = new List<Task<IOrder>>();

    // The sell task loop ..
    foreach (KeyValuePair<Allocation, decimal> allocQuoteDiff in allocQuoteDiffs)
    {
      if (allocQuoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency))
      {
        // We can't sell quote currency for quote currency.
        continue;
      }

      // Positive quote differences refer to oversized allocations,
      // and check if reached minimum order size.
      if (allocQuoteDiff.Value >= @this.MinimumOrderSize)
      {
        // Sell ..
        sellTasks.Add(@this.NewOrder(@this.ConstructSellOrder(allocQuoteDiff.Key, allocQuoteDiff.Value))
          // Continue to verify sell order ended, within same task to optimize performance.
          .ContinueWith(sellTask => @this.VerifyOrderEnded(sellTask.Result)).Unwrap());
      }
    }

    return await Task.WhenAll(sellTasks);
  }

  /// <summary>
  /// Sell pieces of oversized <see cref="Allocation"/>s in order for those to meet <paramref name="newAssetAllocs"/>.
  /// Completes when verified that all triggered sell orders are ended.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newAssetAllocs"></param>
  /// <param name="curBalance"></param>
  /// <returns></returns>
  public static async Task<IOrder[]> SellOveragesAndVerify(this IExchangeService @this, IEnumerable<AbsAssetAlloc> newAssetAllocs, Balance? curBalance = null)
  {
    // Fetch balance if not provided.
    curBalance ??= await @this.GetBalance();

    // Get enumerable since we're iterating it just once.
    IEnumerable<KeyValuePair<Allocation, decimal>> allocQuoteDiffs = GetAllocationQuoteDiffs(newAssetAllocs, curBalance);

    return await @this.SellOveragesAndVerify(allocQuoteDiffs);
  }

  /// <summary>
  /// Buy to increase undersized <see cref="Allocation"/>s in order for those to meet <paramref name="newAssetAllocs"/>.
  /// <see cref="Allocation"/> differences are scaled relative to available quote currency.
  /// Completes when all triggered buy orders are posted.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newAssetAllocs"></param>
  /// <returns></returns>
  public static async Task<IOrder[]> BuyUnderages(this IExchangeService @this, IEnumerable<AbsAssetAlloc> newAssetAllocs, Balance? curBalance = null)
  {
    // Fetch balance if not provided.
    curBalance ??= await @this.GetBalance();

    // Initialize quote diff List,
    // being filled using a multi-purpose foreach to eliminate redundant iterations.
    List<KeyValuePair<Allocation, decimal>> allocQuoteDiffs = new();

    // Absolute sum of all negative quote differences,
    // being summed up using a multi-purpose foreach to eliminate redundant iterations.
    decimal totalBuy = 0;

    // Multi-purpose foreach to eliminate redundant iterations.
    foreach (KeyValuePair<Allocation, decimal> allocQuoteDiff in GetAllocationQuoteDiffs(newAssetAllocs, curBalance))
    {
      // Negative quote differences refer to undersized allocations.
      if (allocQuoteDiff.Value < 0)
      {
        // Add to absolute sum of all negative quote differences.
        totalBuy -= allocQuoteDiff.Value;

        // We can't buy quote currency with quote currency.
        if (!allocQuoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency))
        {
          // Add to quote diff List.
          allocQuoteDiffs.Add(allocQuoteDiff);
        }
      }
    }

    // Multiplication ratio to avoid potentially oversized buy order sizes.
    decimal ratio = Math.Min(totalBuy, curBalance.AmountQuote) / totalBuy;

    var buyTasks = new List<Task<IOrder>>();

    // The buy task loop, diffs are already filtered ..
    foreach (KeyValuePair<Allocation, decimal> allocQuoteDiff in allocQuoteDiffs)
    {
      // Scale to avoid potentially oversized buy order sizes.
      // First check eligibility as it is less expensive operation than the multiplication operation.
      decimal amountQuote =
        allocQuoteDiff.Value <= -@this.MinimumOrderSize ? ratio * allocQuoteDiff.Value : 0;

      // Negative quote differences refer to undersized allocations,
      // and check if reached minimum order size.
      if (amountQuote <= -@this.MinimumOrderSize)
      {
        // Buy ..
        buyTasks.Add(@this.NewOrder(@this.ConstructBuyOrder(allocQuoteDiff.Key, Math.Abs(amountQuote))));
      }
    }

    return await Task.WhenAll(buyTasks);
  }

  /// <summary>
  /// Simulate a portfolio rebalance to estimate fees.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newAssetAllocs"></param>
  /// <param name="curBalance"></param>
  /// <returns></returns>
  public static IEnumerable<IOrder> SimulateRebalance(this IExchangeService @this, IEnumerable<AbsAssetAlloc> newAssetAllocs, Balance curBalance)
  {
    // Get enumerable since we're iterating it just once.
    IEnumerable<KeyValuePair<Allocation, decimal>> allocQuoteDiffs = GetAllocationQuoteDiffs(newAssetAllocs, curBalance);

    // Amount of quote currency available.
    decimal quoteAvailable = 0;

    // Absolute sum of all negative quote differences.
    decimal totalBuy = 0;

    List<KeyValuePair<Allocation, decimal>> negativeDiffs = new();

    // The sell order, and buy order prep, loop ..
    foreach (KeyValuePair<Allocation, decimal> allocQuoteDiff in allocQuoteDiffs)
    {
      // Positive quote differences refer to oversized allocations,
      // and check if reached minimum order size.
      if (allocQuoteDiff.Value >= @this.MinimumOrderSize)
      {
        // Add to amount of quote currency available.
        quoteAvailable += allocQuoteDiff.Value;

        // We can't sell quote currency for quote currency.
        if (!allocQuoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency))
        {
          IOrder sellOrder = @this.ConstructSellOrder(allocQuoteDiff.Key, allocQuoteDiff.Value);

          sellOrder.Status = Abstracts.Enums.OrderStatus.Filled;

          // Sell ..
          yield return sellOrder;
        }
      }

      // Negative quote differences refer to undersized allocations.
      else if (allocQuoteDiff.Value <= 0)
      {
        // Add to absolute sum of all negative quote differences.
        totalBuy -= allocQuoteDiff.Value;

        // We can't buy quote currency with quote currency.
        if (!allocQuoteDiff.Key.Market.BaseCurrency.Equals(@this.QuoteCurrency))
        {
          // Add to quote diff List.
          negativeDiffs.Add(allocQuoteDiff);
        }
      }
    }

    // Multiplication ratio to avoid potentially oversized buy order sizes.
    decimal ratio = Math.Min(totalBuy, quoteAvailable) / totalBuy;

    // The buy order loop, diffs are already filtered ..
    foreach (KeyValuePair<Allocation, decimal> allocQuoteDiff in negativeDiffs)
    {
      // Scale to avoid potentially oversized buy order sizes.
      // First check eligibility as it is less expensive operation than the multiplication operation.
      decimal amountQuote =
        allocQuoteDiff.Value <= -@this.MinimumOrderSize ? ratio * allocQuoteDiff.Value : 0;

      // Negative quote differences refer to undersized allocations,
      // and check if reached minimum order size.
      if (amountQuote <= -@this.MinimumOrderSize)
      {
        IOrder buyOrder = @this.ConstructBuyOrder(allocQuoteDiff.Key, Math.Abs(allocQuoteDiff.Value));

        buyOrder.Status = Abstracts.Enums.OrderStatus.Filled;

        // Buy ..
        yield return buyOrder;
      }
    }
  }

  /// <summary>
  /// Asynchronously performs a portfolio rebalance.
  /// </summary>
  /// <param name="this"></param>
  /// <param name="newAssetAllocs"></param>
  /// <param name="allocQuoteDiffs"></param>
  public static async Task<IEnumerable<IOrder>> Rebalance(
    this IExchangeService @this,
    IEnumerable<AbsAssetAlloc> newAssetAllocs,
    IEnumerable<KeyValuePair<Allocation, decimal>>? allocQuoteDiffs = null)
  {
    // Clear the path ..
    await @this.CancelAllOpenOrders();

    // Sell pieces of oversized allocations first,
    // so we have sufficient quote currency available to buy with.
    IOrder[] sellResults = null != allocQuoteDiffs
      ? await @this.SellOveragesAndVerify(allocQuoteDiffs)
      : await @this.SellOveragesAndVerify(newAssetAllocs);

    // Then buy to increase undersized allocations.
    IOrder[] buyResults = await @this.BuyUnderages(newAssetAllocs);

    return sellResults.Concat(buyResults);
  }
}