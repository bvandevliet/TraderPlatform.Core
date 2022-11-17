using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IMarket"/>
public class Market : IMarket
{
  /// <inheritdoc/>
  public IAsset QuoteCurrency { get; set; }

  /// <inheritdoc/>
  public IAsset BaseCurrency { get; set; }

  public Market(IAsset quoteCurrency, IAsset baseCurrency)
  {
    QuoteCurrency = quoteCurrency;
    BaseCurrency = baseCurrency;
  }
}