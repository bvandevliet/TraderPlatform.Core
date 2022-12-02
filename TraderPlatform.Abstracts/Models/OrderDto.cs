using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IOrder"/>
public class OrderDto // : IOrder, IOrderArgs
{
  /// <inheritdoc cref="Order.Id"/>
  public string? Id { get; set; }

  /// <inheritdoc cref="IMarket.QuoteCurrency"/>
  [Required]
  public string QuoteSymbol { get; set; } = null!;

  /// <inheritdoc cref="IMarket.BaseCurrency"/>
  [Required]
  public string BaseSymbol { get; set; } = null!;

  /// <inheritdoc cref="Order.Side"/>
  [Required]
  public OrderSide Side { get; set; }

  /// <inheritdoc cref="Order.Type"/>
  [Required]
  public OrderType Type { get; set; }

  /// <inheritdoc cref="Order.Status"/>
  public OrderStatus Status { get; set; }

  /// <inheritdoc cref="Order.Price"/>
  public decimal? Price { get; set; }

  /// <inheritdoc cref="Order.Amount"/>
  public decimal? Amount { get; set; }

  /// <inheritdoc cref="Order.AmountQuote"/>
  [Required]
  public decimal AmountQuote { get; set; }

  /// <inheritdoc cref="Order.TimeInForce"/>
  public TimeInForce TimeInForce { get; set; }

  /// <inheritdoc cref="Order.AmountFilled"/>
  public decimal AmountFilled { get; set; }

  /// <inheritdoc cref="Order.AmountQuoteFilled"/>
  public decimal AmountQuoteFilled { get; set; }

  /// <inheritdoc cref="Order.AmountRemaining"/>
  public decimal AmountRemaining { get; set; }

  /// <inheritdoc cref="Order.AmountQuoteRemaining"/>
  public decimal AmountQuoteRemaining { get; set; }

  /// <inheritdoc cref="Order.FeeExpected"/>
  public decimal FeeExpected { get; set; }

  /// <inheritdoc cref="Order.FeePaid"/>
  public decimal FeePaid { get; set; }

  /// <inheritdoc cref="Order.Created"/>
  public DateTime Created { get; set; }

  /// <inheritdoc cref="Order.Updated"/>
  public DateTime Updated { get; set; }
}