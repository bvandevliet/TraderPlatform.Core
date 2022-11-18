using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IAsset"/>
public class Asset : IAsset
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
}