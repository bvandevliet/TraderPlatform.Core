using Microsoft.Net.Http.Headers;
using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;

namespace TraderPlatform.Common.Exchanges;

/// <inheritdoc cref="IExchangeService"/>
public class BitvavoExchange : IExchangeService
{
  private readonly HttpClient httpClient;

  /// <inheritdoc/>
  public Asset QuoteCurrency { get; } = new("EUR", "Euro");

  /// <inheritdoc/>
  public decimal MinimumOrderSize { get; } = 5;

  /// <inheritdoc/>
  public decimal MakerFee { get; } = .0015m;

  /// <inheritdoc/>
  public decimal TakerFee { get; } = .0025m;

  public BitvavoExchange(HttpClient httpClient)
  {
    this.httpClient = httpClient;

    this.httpClient.BaseAddress = new("https://api.bitvavo.com/v2/");

    this.httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
  }

  /// <inheritdoc/>
  public Task<Balance> GetBalance()
  {
    throw new NotImplementedException();
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
  public Task<object> GetCandles(Market market, CandleInterval interval, int limit)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<bool> IsTradable(Market market)
  {
    return Task.FromResult(true);
  }

  /// <inheritdoc/>
  public Task<ITickerPrice> GetPrice(Market market)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder> NewOrder(IOrder order)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder> NewOrder(Market market, OrderSide side, OrderType type, IOrderArgs orderArgs)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder?> GetOrder(string orderId, Market? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder?> CancelOrder(string orderId, Market? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> GetOpenOrders(Market? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> CancelAllOpenOrders(Market? market = null)
  {
    return Task.FromResult((IEnumerable<IOrder>)new List<Order>());
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> SellAllPositions(Asset? asset = null)
  {
    throw new NotImplementedException();
  }
}