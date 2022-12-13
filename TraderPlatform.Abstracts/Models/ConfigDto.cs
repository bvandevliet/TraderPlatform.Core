using System.ComponentModel.DataAnnotations;

namespace TraderPlatform.Abstracts.Models;

public abstract class ConfigDto
{
  [Required]
  public AssetDto QuoteCurrency { get; set; } = null!;

  [Range(0, 100)]
  public decimal QuoteAllocation { get; set; } = 0;

  public Dictionary<AssetDto, decimal> AltWeightingFactors { get; set; } = new();

  public List<string> TagsToIgnore { get; set; } = new() { "stablecoin" };

  [Range(1, 100)]
  public int TopRankingCount { get; set; } = 10;

  [Range(1, 50)]
  public decimal Smoothing { get; set; } = 4;

  [Range(1, 20)]
  public decimal NthRoot { get; set; } = 2.5m;

  [Range(1, double.PositiveInfinity)]
  public decimal IntervalHours { get; set; } = 6;

  [Range(0, double.PositiveInfinity)]
  public decimal MinimumDiffQuote { get; set; } = 5;

  [Range(0, 100)]
  public decimal MinimumDiffAllocation { get; set; } = 1.2m;

  public bool AutomationEnabled { get; set; } = false;

  public DateTime? LastRebalance { get; set; } = null;
}