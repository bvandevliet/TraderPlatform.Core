using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IMarketData"/>
public class MarketData : Market, IMarketData
{
  /// <summary>
  /// <inheritdoc cref="IMarketData"/>
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="Market.QuoteCurrency"/></param>
  /// <param name="baseCurrency"><inheritdoc cref="Market.BaseCurrency"/></param>
  public MarketData(IAsset quoteCurrency, IAsset baseCurrency) : base(quoteCurrency, baseCurrency)
  {
  }

  /// <inheritdoc/>
  public int PricePrecision { get; set; }

  /// <inheritdoc/>
  public decimal MinOrderInQuoteCurrency { get; set; }

  /// <inheritdoc/>
  public decimal MinOrderInBaseCurrency { get; set; }
}