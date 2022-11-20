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
    obj is not null and IMarket o
      && QuoteCurrency.Symbol == o.QuoteCurrency.Symbol
      && BaseCurrency.Symbol == o.BaseCurrency.Symbol;

  public override int GetHashCode() =>
    $"{QuoteCurrency.Symbol}{BaseCurrency.Symbol}".GetHashCode();

  public static bool operator ==(Market a, Market b) => a.Equals(b);
  public static bool operator !=(Market a, Market b) => !(a == b);
}