using TraderPlatform.Abstracts.Enums;

namespace TraderPlatform.Abstracts.Interfaces;

public interface IOrder
{
  string? Id { get; }

  IMarket Market { get; set; }

  OrderSide Side { get; set; }

  OrderType Type { get; set; }

  OrderStatus Status { get; }

  decimal Price { get; set; }

  decimal Amount { get; set; }

  decimal AmountFilled { get; }

  decimal AmountRemaining { get; }

  decimal AmountQuote { get; set; }

  decimal AmountQuoteFilled { get; }

  decimal AmountQuoteRemaining { get; }

  decimal FeeExpected { get; }

  decimal FeePaid { get; }

  DateTime Created { get; }

  DateTime Updated { get; }

  bool DisableMarketProtection { get; set; }
}