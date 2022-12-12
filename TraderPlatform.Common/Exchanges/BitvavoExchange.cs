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
  public IAsset QuoteCurrency { get; } = new Asset("EUR", "Euro");

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
    throw new NotImplementedException();
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