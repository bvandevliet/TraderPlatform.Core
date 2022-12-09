namespace TraderPlatform.Abstracts.Interfaces;

public interface IConfig
{
  IAsset QuoteCurrency { get; set; }

  decimal QuoteAllocation { get; set; }

  Dictionary<IAsset, decimal> AltWeightingFactors { get; set; }

  ICollection<string> TagsToIgnore { get; set; }

  int TopRankingCount { get; set; }

  decimal Smoothing { get; set; }

  decimal NthRoot { get; set; }

  decimal IntervalHours { get; set; }

  decimal MinimumDiffQuote { get; set; }

  decimal MinimumDiffAllocation { get; set; }

  bool AutomationEnabled { get; set; }

  DateTime? LastRebalance { get; set; }
}