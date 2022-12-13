using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Pair of <see cref="Asset"/> instances, one for quote currency and one for base currency.
/// </summary>
public class Market : IEquatable<Market>
{
  /// <summary>
  /// The quote currency to value <see cref="BaseCurrency"/> against.
  /// </summary>
  public Asset QuoteCurrency { get; set; }

  /// <summary>
  /// The base currency valued by <see cref="QuoteCurrency"/>.
  /// </summary>
  public Asset BaseCurrency { get; set; }

  /// <summary>
  /// <inheritdoc cref="Market"/>
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="QuoteCurrency"/></param>
  /// <param name="baseCurrency"><inheritdoc cref="BaseCurrency"/></param>
  public Market(Asset quoteCurrency, Asset baseCurrency)
  {
    QuoteCurrency = quoteCurrency;
    BaseCurrency = baseCurrency;
  }

  public override bool Equals(object? obj)
  {
    return Equals(obj as Market);
  }

  public bool Equals(Market? obj) =>
    obj is not null
      && QuoteCurrency.Symbol == obj.QuoteCurrency.Symbol
      && BaseCurrency.Symbol == obj.BaseCurrency.Symbol;

  public override int GetHashCode() =>
    $"{QuoteCurrency.Symbol}{BaseCurrency.Symbol}".GetHashCode();

  public static bool operator ==(Market a, Market b) => a.Equals(b);
  public static bool operator !=(Market a, Market b) => !(a == b);
}