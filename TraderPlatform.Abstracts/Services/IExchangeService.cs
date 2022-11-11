using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Services;

public interface IExchangeService
{
  string QuoteCurrency { get; }

  decimal MinimumOrderSize { get; }

  decimal MakerFee { get; }

  decimal TakerFee { get; }

  IPortfolioBalance GetBalance();

  object DepositHistory();

  object WithdrawHistory();

  object GetCandlesticks(IMarket market, CandlestickInterval interval, int limit);

  bool IsTradable(IMarket market);

  IOrder NewOrder(IOrder order);

  IOrder? GetOrder(IMarket market, string orderId);

  IOrder? CancelOrder(IMarket market, string orderId);

  IEnumerable<IOrder> GetOpenOrders();

  IEnumerable<IOrder> CancelAllOrders();

  IEnumerable<IOrder> SellWholePortfolio();
}