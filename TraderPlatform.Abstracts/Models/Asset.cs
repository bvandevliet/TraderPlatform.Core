namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Represents a currency.
/// </summary>
public class Asset : IEquatable<Asset>
{
  /// <summary>
  /// Short version of the asset name used in market names.
  /// </summary>
  public string Symbol { get; set; }

  /// <summary>
  /// The full name of the asset.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// <inheritdoc cref="Asset"/>
  /// </summary>
  /// <param name="symbol"><inheritdoc cref="Symbol"/></param>
  /// <param name="name"><inheritdoc cref="Name"/></param>
  public Asset(string symbol, string? name = null)
  {
    Symbol = symbol;
    Name = name ?? string.Empty;
  }

  public override bool Equals(object? obj)
  {
    return Equals(obj as Asset);
  }

  public bool Equals(Asset? obj) =>
    obj is not null && Symbol == obj.Symbol;

  public override int GetHashCode() => Symbol.GetHashCode();

  public static bool operator ==(Asset a, Asset b) => a.Symbol == b.Symbol;
  public static bool operator !=(Asset a, Asset b) => !(a == b);
}