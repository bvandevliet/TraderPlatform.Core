using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Services;

public interface IExchangeService
{
  IAsset QuoteCurrency { get; }

  decimal MinimumOrderSize { get; }

  decimal MakerFee { get; }

  decimal TakerFee { get; }

  Task<Balance> GetBalance();

  // TODO: ASIGN TYPE !!
  Task<object> DepositHistory();

  // TODO: ASIGN TYPE !!
  Task<object> WithdrawHistory();

  // TODO: ASIGN TYPE !!
  Task<object> GetCandles(IMarket market, CandleInterval interval, int limit);

  Task<bool> IsTradable(IMarket market);

  Task<ITickerPrice> GetPrice(IMarket market);

  Task<IOrder> NewOrder(IMarket market, OrderSide side, OrderType type, IOrderArgs orderArgs);

  Task<IOrder?> GetOrder(string orderId, IMarket? market);

  Task<IOrder?> CancelOrder(string orderId, IMarket? market);

  Task<IEnumerable<IOrder>> GetOpenOrders(IMarket? market);

  Task<IEnumerable<IOrder>> CancelAllOpenOrders(IMarket? market);

  Task<IEnumerable<IOrder>> SellAllPositions(IAsset? asset);
}