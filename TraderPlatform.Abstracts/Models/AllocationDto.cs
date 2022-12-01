using System.ComponentModel.DataAnnotations;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="Allocation"/>
public class AllocationDto // : Allocation, ITickerPrice, IPosition
{
  /// <inheritdoc cref="Allocation.Market"/>
  [Required]
  public MarketDto Market { get; set; } = null!;

  /// <inheritdoc cref="Allocation.Price"/>
  [Required]
  public decimal Price { get; set; }

  /// <inheritdoc cref="Allocation.Amount"/>
  [Required]
  public decimal Amount { get; set; }

  /// <inheritdoc cref="Allocation.AmountQuote"/>
  [Required]
  public decimal AmountQuote { get; set; }
}