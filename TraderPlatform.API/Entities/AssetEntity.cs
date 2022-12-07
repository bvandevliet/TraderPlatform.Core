using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.API.Entities;

/// <inheritdoc cref="IAsset"/>
public class AssetEntity : IAsset
{
  /// <inheritdoc cref="IAsset.Symbol"/>
  [Required]
  [MaxLength(20)]
  public string Symbol { get; set; }

  /// <inheritdoc cref="IAsset.Name"/>
  [MaxLength(50)]
  public string Name { get; set; }

  /// <inheritdoc cref="Asset(string, string?)"/>
  public AssetEntity(string symbol, string? name = null)
  {
    Symbol = symbol;
    Name = name ?? string.Empty;
  }

  public bool Equals(IAsset? other)
  {
    throw new NotImplementedException();
  }
}