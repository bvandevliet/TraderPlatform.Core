namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Represents a currency.
/// </summary>
public interface IAsset : IEquatable<IAsset>
{
  /// <summary>
  /// Short version of the asset name used in market names.
  /// </summary>
  string Symbol { get; }

  /// <summary>
  /// The full name of the asset.
  /// </summary>
  string Name { get; }
}