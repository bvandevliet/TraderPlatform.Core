using System.ComponentModel.DataAnnotations;

namespace TraderPlatform.Abstracts.Models;

public class RebalanceTriggerDto
{
  /// <inheritdoc cref="AbsAssetAllocDto"/>
  [Required]
  public IEnumerable<AbsAssetAllocDto> NewAssetAllocs { get; set; } = null!;

  /// <inheritdoc cref="AllocationDto"/>
  public IEnumerable<KeyValuePair<AllocationDto, decimal>>? AllocQuoteDiffs { get; set; } = null;
}