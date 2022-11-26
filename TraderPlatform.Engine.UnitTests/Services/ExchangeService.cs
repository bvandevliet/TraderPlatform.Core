using System.Collections.Generic;
using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;

namespace TraderPlatform.Engine.UnitTests.Services;

/// <inheritdoc cref="IExchangeService"/>
internal class ExchangeService : IExchangeService
{
  /// <inheritdoc/>
  public IAsset QuoteCurrency { get; }

  /// <inheritdoc/>
  public decimal MinimumOrderSize { get; }

  /// <inheritdoc/>
  public decimal MakerFee { get; }

  /// <inheritdoc/>
  public decimal TakerFee { get; }

  /// <summary>
  /// <inheritdoc cref="IExchangeService"/>
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="QuoteCurrency"/></param>
  /// <param name="minimumOrderSize"><inheritdoc cref="MinimumOrderSize"/></param>
  /// <param name="makerFee"><inheritdoc cref="MakerFee"/></param>
  /// <param name="takerFee"><inheritdoc cref="TakerFee"/></param>
  public ExchangeService(
    IAsset quoteCurrency,
    decimal minimumOrderSize,
    decimal makerFee,
    decimal takerFee)
  {
    QuoteCurrency = quoteCurrency;
    MinimumOrderSize = minimumOrderSize;
    MakerFee = makerFee;
    TakerFee = takerFee;
  }

  /// <summary>
  /// Returns a drifted <see cref="Balance"/> that had a initial value of €1000 allocated as follows:
  /// <br/>
  /// EUR :  5 %
  /// <br/>
  /// BTC : 40 %
  /// <br/>
  /// ETH : 30 %
  /// <br/>
  /// BNB : 25 %
  /// </summary>
  /// <returns></returns>
  public Task<Balance> GetBalance()
  {
    decimal deposit = 1000;

    Balance curBalance = new(QuoteCurrency);

    curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("EUR")), 000001, .05m * deposit));
    curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("BTC")), 18_000, .40m * deposit / 15_000));
    curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("ETH")), 01_610, .30m * deposit / 01_400));
    curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("BNB")), 000306, .25m * deposit / 000340));
    //                                                                                100%

    return Task.FromResult(curBalance);
  }

  /// <inheritdoc/>
  public Task<object> DepositHistory()
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<object> WithdrawHistory()
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<object> GetCandles(IMarket market, CandleInterval interval, int limit)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<bool> IsTradable(IMarket market)
  {
    return Task.FromResult(true);
  }

  /// <inheritdoc/>
  public Task<ITickerPrice> GetPrice(IMarket market)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder> NewOrder(IOrder order)
  {
    order.Status = OrderStatus.Filled;

    return Task.FromResult(order);
  }

  /// <inheritdoc/>
  public Task<IOrder> NewOrder(IMarket market, OrderSide side, OrderType type, IOrderArgs orderArgs)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder?> GetOrder(string orderId, IMarket? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder?> CancelOrder(string orderId, IMarket? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> GetOpenOrders(IMarket? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> CancelAllOpenOrders(IMarket? market = null)
  {
    return Task.FromResult((IEnumerable<IOrder>)new List<Order>());
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> SellAllPositions(IAsset? asset = null)
  {
    throw new NotImplementedException();
  }
}