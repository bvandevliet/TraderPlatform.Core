using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Additional market data.
/// </summary>
public class MarketData : Market
{
  /// <summary>
  /// Determines how many significant digits are allowed.
  /// The rationale behind this is that for higher amounts, smaller price increments are less relevant.
  /// </summary>
  public int PricePrecision { get; set; }

  /// <summary>
  /// The minimum amount in quote currency for valid orders.
  /// </summary>
  public decimal MinOrderInQuoteCurrency { get; set; }

  /// <summary>
  /// The minimum amount in base currency for valid orders.
  /// </summary>
  public decimal MinOrderInBaseCurrency { get; set; }

  /// <summary>
  /// <inheritdoc cref="MarketData"/>
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="Market.QuoteCurrency"/></param>
  /// <param name="baseCurrency"><inheritdoc cref="Market.BaseCurrency"/></param>
  public MarketData(Asset quoteCurrency, Asset baseCurrency) : base(quoteCurrency, baseCurrency)
  {
  }
}