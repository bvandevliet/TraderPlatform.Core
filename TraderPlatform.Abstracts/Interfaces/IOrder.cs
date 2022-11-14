using TraderPlatform.Abstracts.Enums;

namespace TraderPlatform.Abstracts.Interfaces;

public interface IOrder
{
  string? Id { get; }

  // SHOULD BE ONLY GET WHEN ORDER IS RETURNED !!
  IMarket Market { get; set; }

  // SHOULD BE ONLY GET WHEN ORDER IS RETURNED !!
  OrderSide Side { get; set; }

  // SHOULD BE ONLY GET WHEN ORDER IS RETURNED !!
  OrderType Type { get; set; }

  OrderStatus Status { get; }

  // SHOULD BE ONLY GET WHEN ORDER IS RETURNED !!
  decimal Price { get; set; }

  // SHOULD BE ONLY GET WHEN ORDER IS RETURNED !!
  decimal Amount { get; set; }

  decimal AmountFilled { get; }

  decimal AmountRemaining { get; }

  // SHOULD BE ONLY GET WHEN ORDER IS RETURNED !!
  decimal AmountQuote { get; set; }

  decimal AmountQuoteFilled { get; }

  decimal AmountQuoteRemaining { get; }

  decimal FeeExpected { get; }

  decimal FeePaid { get; }

  DateTime Created { get; }

  DateTime Updated { get; }

  // SHOULD BE ONLY GET WHEN ORDER IS RETURNED !!
  bool DisableMarketProtection { get; set; }
}