namespace TraderPlatform.Abstracts.Interfaces;

public interface IOrderArgs
{
  /// <summary>
  /// Only for limit orders. Specifies the amount in quote currency that is paid/received for each unit of base currency.
  /// </summary>
  public decimal? Price { get; set; }

  /// <summary>
  /// Only for limit orders. Specifies the amount of base currency that will be bought/sold.
  /// </summary>
  public decimal? Amount { get; set; }

  /// <summary>
  /// Only for market orders. If specified, <see cref="AmountQuote"/> of the quote currency will be bought/sold for the best price available.
  /// </summary>
  public decimal? AmountQuote { get; set; }

  /// <inheritdoc cref="Enums.TimeInForce"/>
  public Enums.TimeInForce TimeInForce { get; set; }
}