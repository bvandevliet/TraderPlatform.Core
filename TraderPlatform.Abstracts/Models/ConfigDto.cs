using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IConfig"/>
public abstract class ConfigDto : IConfig
{
  /// <inheritdoc/>
  [Required]
  public IAsset QuoteCurrency { get; set; } = null!;

  /// <inheritdoc/>
  [Range(0, 100)]
  public decimal QuoteAllocation { get; set; } = 0;

  /// <inheritdoc/>
  public Dictionary<IAsset, decimal> AltWeightingFactors { get; set; } = new();

  /// <inheritdoc/>
  public List<string> TagsToIgnore { get; set; } = new() { "stablecoin" };

  /// <inheritdoc/>
  [Range(1, 100)]
  public int TopRankingCount { get; set; } = 10;

  /// <inheritdoc/>
  [Range(1, 50)]
  public decimal Smoothing { get; set; } = 4;

  /// <inheritdoc/>
  [Range(1, 20)]
  public decimal NthRoot { get; set; } = 2.5m;

  /// <inheritdoc/>
  [Range(1, double.PositiveInfinity)]
  public decimal IntervalHours { get; set; } = 6;

  /// <inheritdoc/>
  [Range(0, double.PositiveInfinity)]
  public decimal MinimumDiffQuote { get; set; } = 5;

  /// <inheritdoc/>
  [Range(0, 100)]
  public decimal MinimumDiffAllocation { get; set; } = 1.2m;

  /// <inheritdoc/>
  public bool AutomationEnabled { get; set; } = false;

  /// <inheritdoc/>
  public DateTime? LastRebalance { get; set; } = null;
}