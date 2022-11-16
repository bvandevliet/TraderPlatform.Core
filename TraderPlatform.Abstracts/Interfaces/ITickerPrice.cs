namespace TraderPlatform.Abstracts.Interfaces;

public interface ITickerPrice
{
  /// <inheritdoc cref="IMarket"/>
  IMarket Market { get; }

  /// <summary>
  /// Price in quote currency per unit of base currency.
  /// </summary>
  decimal Price { get; }
}