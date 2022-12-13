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
  /// <param name="orderArgs"><inheritdoc cref="IOrderArgs"/></param>
  public Order(
    Market market,
    OrderSide side,
    OrderType type,
    IOrderArgs orderArgs)
  {
    Market = market;
    Side = side;
    Type = type;

    Price = orderArgs.Price;
    Amount = orderArgs.Amount;
    AmountQuote = orderArgs.AmountQuote;
    TimeInForce = orderArgs.TimeInForce;
  }

  /// <summary>
  /// <inheritdoc cref="Order(Market, OrderSide, OrderType, IOrderArgs)"/>
  /// </summary>
  /// <param name="quoteSymbol"><inheritdoc cref="Market.QuoteCurrency"/></param>
  /// <param name="baseSymbol"><inheritdoc cref="Market.BaseCurrency"/></param>
  /// <param name="side"><inheritdoc cref="Side"/></param>
  /// <param name="type"><inheritdoc cref="Type"/></param>
  /// <param name="orderArgs"><inheritdoc cref="IOrderArgs"/></param>
  public Order(
    string quoteSymbol,
    string baseSymbol,
    OrderSide side,
    OrderType type,
    IOrderArgs orderArgs)
    : this(
        new Market(new Asset(quoteSymbol), new Asset(baseSymbol)),
        side, type, orderArgs)
  {
  }

  /// <summary>
  /// <inheritdoc cref="Order(Market, OrderSide, OrderType, IOrderArgs)"/>
  /// </summary>
  /// <param name="quoteSymbol"><inheritdoc cref="Market.QuoteCurrency"/></param>
  /// <param name="baseSymbol"><inheritdoc cref="Market.BaseCurrency"/></param>
  /// <param name="side"><inheritdoc cref="Side"/></param>
  /// <param name="type"><inheritdoc cref="Type"/></param>
  /// <param name="amountQuote"><inheritdoc cref="IOrderArgs.AmountQuote"/></param>
  /// <param name="amount"><inheritdoc cref="IOrderArgs.Amount"/></param>
  /// <param name="price"><inheritdoc cref="IOrderArgs.Price"/></param>
  public Order(
    string quoteSymbol,
    string baseSymbol,
    OrderSide side,
    OrderType type,
    decimal amountQuote,
    decimal? amount = null,
    decimal? price = null)
    : this(
        new Market(new Asset(quoteSymbol), new Asset(baseSymbol)),
        side, type,
        new OrderArgs(amountQuote) { Amount = amount, Price = price })
  {
  }

  /// <inheritdoc/>
  public string? Id { get; set; }

  /// <inheritdoc/>
  public Market Market { get; }

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