using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IAsset"/>
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
public class Asset : IAsset
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
{
  /// <inheritdoc/>
  public string Symbol { get; set; }

  /// <inheritdoc/>
  public string Name { get; set; }

  /// <summary>
  /// <inheritdoc cref="IAsset"/>
  /// </summary>
  /// <param name="symbol"><inheritdoc cref="Symbol"/></param>
  /// <param name="name"><inheritdoc cref="Name"/></param>
  public Asset(string symbol, string? name = null)
  {
    Symbol = symbol;
    Name = name ?? string.Empty;
  }

  public bool Equals(IAsset? obj) =>
    obj is not null && Symbol == obj.Symbol;

  public override int GetHashCode() => Symbol.GetHashCode();

  public static bool operator ==(Asset a, Asset b) => a.Symbol == b.Symbol;
  public static bool operator !=(Asset a, Asset b) => !(a == b);
}