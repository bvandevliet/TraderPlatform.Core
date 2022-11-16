namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Represents the value of a given base currency against a quote currency.
/// </summary>
public interface ITickerPrice
{
  /// <inheritdoc cref="IMarket"/>
  IMarket Market { get; }

  /// <summary>
  /// Price in quote currency per unit of base currency.
  /// </summary>
  decimal Price { get; }
}