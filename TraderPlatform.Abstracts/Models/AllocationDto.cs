using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="Allocation"/>
public class AllocationDto // : Allocation, ITickerPrice, IPosition
{
  /// <inheritdoc cref="IMarket.QuoteCurrency"/>
  [Required]
  public string QuoteSymbol { get; set; } = null!;

  /// <inheritdoc cref="IMarket.BaseCurrency"/>
  [Required]
  public string BaseSymbol { get; set; } = null!;

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