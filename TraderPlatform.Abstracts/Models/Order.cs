using System.Diagnostics;
using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IOrder"/>
public class Order : IOrder, IOrderArgs
{
  /// <summary>
  /// <inheritdoc cref="IOrder"/>
  /// </summary>
  /// <param name="market"><inheritdoc cref="Market"/></param>
  /// <param name="side"><inheritdoc cref="Side"/></param>
  /// <param name="type"><inheritdoc cref="Type"/></param>
  public Order(IMarket market, OrderSide side, OrderType type, IOrderArgs orderArgs)
  {
    Market = market;
    Side = side;
    Type = type;

    Price = orderArgs.Price;
    Amount = orderArgs.Amount;
    AmountQuote = orderArgs.AmountQuote;
    TimeInForce = orderArgs.TimeInForce;
  }

  /// <inheritdoc/>
  public string? Id { get; set; }

  /// <inheritdoc/>
  public IMarket Market { get; }

  /// <inheritdoc/>
  public OrderSide Side { get; }

  /// <inheritdoc/>
  public OrderType Type { get; }

  /// <inheritdoc/>
  public OrderStatus Status { get; set; }

  /// <inheritdoc/>
  public decimal? Price { get; set; }

  /// <inheritdoc/>
  public decimal? Amount { get; set; }

  /// <inheritdoc/>
  public decimal AmountQuote { get; set; }

  /// <inheritdoc/>
  public TimeInForce TimeInForce { get; set; }

  /// <inheritdoc/>
  public decimal AmountFilled { get; set; }

  /// <inheritdoc/>
  public decimal AmountQuoteFilled { get; set; }

  /// <inheritdoc/>
  public decimal AmountRemaining { get; set; }

  /// <inheritdoc/>
  public decimal AmountQuoteRemaining { get; set; }

  /// <inheritdoc/>
  public decimal FeeExpected { get; set; }

  /// <inheritdoc/>
  public decimal FeePaid { get; set; }

  /// <inheritdoc/>
  public DateTime Created { get; set; }

  /// <inheritdoc/>
  public DateTime Updated { get; set; }
}