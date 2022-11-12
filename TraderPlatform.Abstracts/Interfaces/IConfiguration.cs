using System.ComponentModel.DataAnnotations;

namespace TraderPlatform.Abstracts.Interfaces;

// DATA ANNOTATIONS WITHIN INTERFACE ??
public interface IConfiguration
{
  IAsset QuoteCurrency { get; set; }

  [Range(0, 100)]
  decimal QuoteAllocation { get; set; }

  [Range(1, 100)]
  int TopRankingCount { get; set; }

  [Range(1, 50)]
  decimal Smoothing { get; set; }

  [Range(1, 20)]
  decimal NthRoot { get; set; }

  [Range(1, double.PositiveInfinity)]
  decimal IntervalHours { get; set; }

  [Range(0, double.PositiveInfinity)]
  decimal MinimumDiffQuote { get; set; }

  [Range(0, 100)]
  decimal MinimumDiffAllocation { get; set; }

  bool AutomationEnabled { get; set; }

  DateTime? LastRebalance { get; set; }
}