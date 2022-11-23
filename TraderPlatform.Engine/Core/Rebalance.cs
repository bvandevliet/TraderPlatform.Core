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

  public static IEnumerable<KeyValuePair<Allocation, decimal>> GetAllocationQuoteDiffs(Balance newBalance, Balance currentBalance)
  {
    foreach (Allocation curAlloc in currentBalance.Allocations)
    {
      Allocation fallbackAlloc = new(curAlloc.Market, curAlloc.Price, 0);

      Allocation newAlloc = newBalance.Allocations.FirstOrDefault(alloc => alloc.Market.Equals(curAlloc.Market), fallbackAlloc);

      yield return GetAllocationQuoteDiff(newAlloc, curAlloc);
    }

    // Add missing !!
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

  public static async void Rebalance(this IExchangeService @this, Balance newBalance, Balance? currentBalance = null)
  {
    currentBalance ??= await @this.GetBalance();

    var sellTasks = new List<Task<IOrder>>();

    foreach (KeyValuePair<Allocation, decimal> quoteDiff in GetAllocationQuoteDiffs(newBalance, currentBalance))
    {
      if (quoteDiff.Value > 0)
      {
        // Need to sell since we own too many.
        sellTasks.Add(@this.NewOrder(
          quoteDiff.Key.Market,
          Abstracts.Enums.OrderSide.Sell,
          Abstracts.Enums.OrderType.Market,
          new OrderArgs
          {
            AmountQuote = Math.Abs(quoteDiff.Value),
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

    // Compensate for spread/slippage ..

    // Buy ..
  }
}