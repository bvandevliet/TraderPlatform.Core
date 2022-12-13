using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Services;

public interface IExchangeService
{
  Asset QuoteCurrency { get; }

  decimal MinimumOrderSize { get; }

  decimal MakerFee { get; }

  decimal TakerFee { get; }

  Task<Balance> GetBalance();

  // TODO: ASIGN TYPE !!
  Task<object> DepositHistory();

  // TODO: ASIGN TYPE !!
  Task<object> WithdrawHistory();

  // TODO: ASIGN TYPE !!
  Task<object> GetCandles(Market market, CandleInterval interval, int limit);

  Task<bool> IsTradable(Market market);

  Task<ITickerPrice> GetPrice(Market market);

  Task<IOrder> NewOrder(IOrder order);

  Task<IOrder> NewOrder(Market market, OrderSide side, OrderType type, IOrderArgs orderArgs);

  Task<IOrder?> GetOrder(string orderId, Market? market = null);

  Task<IOrder?> CancelOrder(string orderId, Market? market = null);

  Task<IEnumerable<IOrder>> GetOpenOrders(Market? market = null);

  Task<IEnumerable<IOrder>> CancelAllOpenOrders(Market? market = null);

  Task<IEnumerable<IOrder>> SellAllPositions(Asset? asset = null);
}