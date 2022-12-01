using System.ComponentModel.DataAnnotations;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="AbsAssetAlloc"/>
public class AbsAssetAllocDto // : AbsAssetAlloc
{
  /// <inheritdoc cref="AbsAssetAlloc.Asset"/>
  [Required]
  public AssetDto Asset { get; set; } = null!;

  /// <inheritdoc cref="AbsAssetAlloc.AbsAlloc"/>
  [Required]
  public decimal AbsAlloc { get; set; }
}