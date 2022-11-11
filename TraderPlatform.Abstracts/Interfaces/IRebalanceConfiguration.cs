namespace TraderPlatform.Abstracts.Interfaces;

public interface IRebalanceConfiguration
{
  string? QuoteCurrency { get; set; }

  decimal QuoteAllocation { get; set; }

  int TopRankingCount { get; set; }

  decimal Smoothing { get; set; }

  decimal NthRoot { get; set; }

  decimal IntervalHours { get; set; }

  decimal MinimumDiffQuote { get; set; }

  decimal MinimumDiffAllocation { get; set; }

  bool AutomationEnabled { get; set; }

  DateTime? LastRebalance { get; set; }
}