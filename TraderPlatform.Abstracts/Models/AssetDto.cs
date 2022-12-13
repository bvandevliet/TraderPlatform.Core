using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="Asset"/>
public class AssetDto // : Asset
{
  /// <inheritdoc cref="Asset.Symbol"/>
  [Required]
  [MaxLength(20)]
  public string Symbol { get; set; } = null!;

  /// <inheritdoc cref="Asset.Name"/>
  [MaxLength(50)]
  public string Name { get; set; } = string.Empty;
}