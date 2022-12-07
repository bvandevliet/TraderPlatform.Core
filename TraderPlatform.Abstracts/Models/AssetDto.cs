using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IAsset"/>
public class AssetDto // : IAsset
{
  /// <inheritdoc cref="IAsset.Symbol"/>
  [Required]
  [MaxLength(20)]
  public string Symbol { get; set; } = null!;

  /// <inheritdoc cref="IAsset.Name"/>
  [MaxLength(50)]
  public string Name { get; set; } = string.Empty;
}