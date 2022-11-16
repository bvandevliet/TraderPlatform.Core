using TraderPlatform.Abstracts.Enums;

namespace TraderPlatform.Abstracts.Interfaces;

public interface IOrder : IOrderArgs
{
  string? Id { get; set; }

  IMarket Market { get; }

  OrderSide Side { get; }

  OrderType Type { get; }

  OrderStatus Status { get; set; }

  decimal AmountFilled { get; set; }

  decimal AmountRemaining { get; set; }

  decimal AmountQuoteFilled { get; set; }

  decimal AmountQuoteRemaining { get; set; }

  decimal FeeExpected { get; set; }

  decimal FeePaid { get; set; }

  DateTime Created { get; set; }

  DateTime Updated { get; set; }
}