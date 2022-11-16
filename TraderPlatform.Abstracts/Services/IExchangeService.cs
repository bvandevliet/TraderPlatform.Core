using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Services;

interface IExchangeService
{
  public IAsset QuoteCurrency { get; }

  public decimal MinimumOrderSize { get; }

  public decimal MakerFee { get; }

  public decimal TakerFee { get; }

  internal Task<Balance> GetBalance();

  // TODO: ASIGN TYPE !!
  internal Task<object> DepositHistory();

  // TODO: ASIGN TYPE !!
  internal Task<object> WithdrawHistory();

  // TODO: ASIGN TYPE !!
  internal Task<object> GetCandles(IMarket market, CandleInterval interval, int limit);

  internal Task<bool> IsTradable(IMarket market);

  internal Task<ITickerPrice> GetPrice(IMarket market);

  internal Task<IOrder> NewOrder(IMarket market, OrderSide side, OrderType type, IOrderArgs orderArgs);

  internal Task<IOrder?> GetOrder(string orderId, IMarket? market);

  internal Task<IOrder?> CancelOrder(string orderId, IMarket? market);

  internal Task<IEnumerable<IOrder>> GetOpenOrders(IMarket? market);

  internal Task<IEnumerable<IOrder>> CancelAllOpenOrders(IMarket? market);

  internal Task<IEnumerable<IOrder>> SellAllPositions(IAsset? asset);
}