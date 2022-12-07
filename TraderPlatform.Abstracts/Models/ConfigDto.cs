using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IConfig"/>
public abstract class ConfigDto // : IConfig
{
  /// <inheritdoc cref="IConfig.QuoteCurrency"/>
  [Required]
  public AssetDto QuoteCurrency { get; set; } = null!;

  /// <inheritdoc cref="IConfig.QuoteAllocation"/>
  [Range(0, 100)]
  public decimal QuoteAllocation { get; set; } = 0;

  /// <inheritdoc cref="IConfig.AltWeightingFactors"/>
  public Dictionary<AssetDto, decimal> AltWeightingFactors { get; set; } = new();

  /// <inheritdoc cref="IConfig.TagsToIgnore"/>
  public List<string> TagsToIgnore { get; set; } = new() { "stablecoin" };

  /// <inheritdoc cref="IConfig.TopRankingCount"/>
  [Range(1, 100)]
  public int TopRankingCount { get; set; } = 10;

  /// <inheritdoc cref="IConfig.Smoothing"/>
  [Range(1, 50)]
  public decimal Smoothing { get; set; } = 4;

  /// <inheritdoc cref="IConfig.NthRoot"/>
  [Range(1, 20)]
  public decimal NthRoot { get; set; } = 2.5m;

  /// <inheritdoc cref="IConfig.IntervalHours"/>
  [Range(1, double.PositiveInfinity)]
  public decimal IntervalHours { get; set; } = 6;

  /// <inheritdoc cref="IConfig.MinimumDiffQuote"/>
  [Range(0, double.PositiveInfinity)]
  public decimal MinimumDiffQuote { get; set; } = 5;

  /// <inheritdoc cref="IConfig.MinimumDiffAllocation"/>
  [Range(0, 100)]
  public decimal MinimumDiffAllocation { get; set; } = 1.2m;

  /// <inheritdoc cref="IConfig.AutomationEnabled"/>
  public bool AutomationEnabled { get; set; } = false;

  /// <inheritdoc cref="IConfig.LastRebalance"/>
  public DateTime? LastRebalance { get; set; } = null;
}