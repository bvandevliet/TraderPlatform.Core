using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IMarket"/>
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
public class Market : IMarket
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
{
  /// <inheritdoc/>
  public IAsset QuoteCurrency { get; set; }

  /// <inheritdoc/>
  public IAsset BaseCurrency { get; set; }

  /// <summary>
  /// <inheritdoc cref="IMarket"/>
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="QuoteCurrency"/></param>
  /// <param name="baseCurrency"><inheritdoc cref="BaseCurrency"/></param>
  public Market(IAsset quoteCurrency, IAsset baseCurrency)
  {
    QuoteCurrency = quoteCurrency;
    BaseCurrency = baseCurrency;
  }

  public bool Equals(IMarket? obj) =>
    obj is not null
      && QuoteCurrency.Symbol == obj.QuoteCurrency.Symbol
      && BaseCurrency.Symbol == obj.BaseCurrency.Symbol;

  public override int GetHashCode() =>
    $"{QuoteCurrency.Symbol}{BaseCurrency.Symbol}".GetHashCode();

  public static bool operator ==(Market a, Market b) => a.Equals(b);
  public static bool operator !=(Market a, Market b) => !(a == b);
}