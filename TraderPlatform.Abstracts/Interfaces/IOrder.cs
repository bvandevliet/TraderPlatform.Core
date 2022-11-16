using TraderPlatform.Abstracts.Enums;

namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Represents an order.
/// </summary>
public interface IOrder : IOrderArgs
{
  /// <summary>
  /// Order Id.
  /// </summary>
  string? Id { get; set; }

  /// <summary>
  /// The market to trade.
  /// </summary>
  IMarket Market { get; }

  /// <summary>
  /// When placing a buy order the base currency will be bought for the quote currency.
  /// When placing a sell order the base currency will be sold for the quote currency.
  /// </summary>
  OrderSide Side { get; }

  /// <summary>
  /// For limit orders, <see cref="IOrderArgs.Amount"/> and <see cref="IOrderArgs.Price"/> are required.
  /// For market orders either <see cref="IOrderArgs.Amount"/> or <see cref="IOrderArgs.AmountQuote"/> is required.
  /// </summary>
  OrderType Type { get; }

  /// <summary>
  /// Current status of the order.
  /// </summary>
  OrderStatus Status { get; set; }

  /// <summary>
  /// Amount in base currency filled.
  /// </summary>
  decimal AmountFilled { get; set; }

  /// <summary>
  /// Amount in quote currency filled.
  /// </summary>
  decimal AmountQuoteFilled { get; set; }

  /// <summary>
  /// Only for orders with <see cref="IOrderArgs.Amount"/> supplied.
  /// Amount in base currency remaining (lower than <see cref="IOrderArgs.Amount"/> after fills).
  /// </summary>
  decimal AmountRemaining { get; set; }

  /// <summary>
  /// Only for market orders with <see cref="IOrderArgs.AmountQuote"/> supplied.
  /// Amount in quote currency remaining (lower than <see cref="IOrderArgs.AmountQuote"/> after fills).
  /// </summary>
  decimal AmountQuoteRemaining { get; set; }

  /// <summary>
  /// Expected amount in quote currency to be paid for fee.
  /// </summary>
  decimal FeeExpected { get; set; }

  /// <summary>
  /// Amount in quote currency paid for fee.
  /// </summary>
  decimal FeePaid { get; set; }

  /// <summary>
  /// Timestamp this order was created.
  /// </summary>
  DateTime Created { get; set; }

  /// <summary>
  /// Timestamp this order was updated.
  /// </summary>
  DateTime Updated { get; set; }
}