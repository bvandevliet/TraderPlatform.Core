namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Pair of <see cref="IAsset"/> instances, one for quote currency and one for base currency.
/// </summary>
public interface IMarket
{
  /// <summary>
  /// The quote currency to value <see cref="BaseCurrency"/> against.
  /// </summary>
  IAsset QuoteCurrency { get; }

  /// <summary>
  /// The base currency valued by <see cref="QuoteCurrency"/>.
  /// </summary>
  IAsset BaseCurrency { get; }
}