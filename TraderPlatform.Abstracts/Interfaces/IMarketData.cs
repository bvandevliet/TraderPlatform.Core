namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Additional market data.
/// </summary>
public interface IMarketData : IMarket
{
  /// <summary>
  /// Determines how many significant digits are allowed.
  /// The rationale behind this is that for higher amounts, smaller price increments are less relevant.
  /// </summary>
  int PricePrecision { get; set; }

  /// <summary>
  /// The minimum amount in quote currency for valid orders.
  /// </summary>
  decimal MinOrderInQuoteCurrency { get; set; }

  /// <summary>
  /// The minimum amount in base currency for valid orders.
  /// </summary>
  decimal MinOrderInBaseCurrency { get; set; }
}