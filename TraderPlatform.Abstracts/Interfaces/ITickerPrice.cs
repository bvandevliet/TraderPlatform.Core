using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Represents the value of a given base currency against a quote currency.
/// </summary>
public interface ITickerPrice
{
  /// <inheritdoc cref="Market"/>
  Market Market { get; }

  /// <summary>
  /// Price in quote currency per unit of base currency.
  /// </summary>
  decimal Price { get; }
}