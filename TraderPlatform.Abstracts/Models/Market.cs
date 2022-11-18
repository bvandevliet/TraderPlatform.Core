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

  public override bool Equals(object? obj) =>
    obj is not null and IMarket
      && QuoteCurrency.Symbol == ((IMarket)obj).QuoteCurrency.Symbol
      && BaseCurrency.Symbol == ((IMarket)obj).BaseCurrency.Symbol;

  public override int GetHashCode() =>
    $"{QuoteCurrency.Symbol}{BaseCurrency.Symbol}".GetHashCode();
}