using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.API.Entities;

/// <inheritdoc cref="IConfig"/>
public class ConfigEntity // : IConfig
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  /// <inheritdoc cref="IConfig.QuoteCurrency"/>
  public string QuoteSymbol { get; set; }

  /// <inheritdoc cref="IConfig.QuoteAllocation"/>
  public decimal QuoteAllocation { get; set; }

  /// <inheritdoc cref="IConfig.AltWeightingFactors"/>
  public virtual Dictionary<string, decimal> AltWeightingFactors { get; set; }

  /// <inheritdoc cref="IConfig.TagsToIgnore"/>
  public ICollection<string> TagsToIgnore { get; set; }

  /// <inheritdoc cref="IConfig.TopRankingCount"/>
  public int TopRankingCount { get; set; }

  /// <inheritdoc cref="IConfig.Smoothing"/>
  public decimal Smoothing { get; set; }

  /// <inheritdoc cref="IConfig.NthRoot"/>
  public decimal NthRoot { get; set; }

  /// <inheritdoc cref="IConfig.IntervalHours"/>
  public decimal IntervalHours { get; set; }

  /// <inheritdoc cref="IConfig.MinimumDiffQuote"/>
  public decimal MinimumDiffQuote { get; set; }

  /// <inheritdoc cref="IConfig.MinimumDiffAllocation"/>
  public decimal MinimumDiffAllocation { get; set; }

  /// <inheritdoc cref="IConfig.AutomationEnabled"/>
  public bool AutomationEnabled { get; set; }

  /// <inheritdoc cref="IConfig.LastRebalance"/>
  public DateTime? LastRebalance { get; set; }

  public ConfigEntity(
    string quoteSymbol,
    decimal quoteAllocation,
    //Dictionary<AssetEntity, decimal> altWeightingFactors,
    //List<string> tagsToIgnore,
    int topRankingCount,
    decimal smoothing,
    decimal nthRoot,
    decimal intervalHours,
    decimal minimumDiffQuote,
    decimal minimumDiffAllocation,
    bool automationEnabled,
    DateTime? lastRebalance = null)
  {
    QuoteSymbol = quoteSymbol;
    QuoteAllocation = quoteAllocation;
    //AltWeightingFactors = altWeightingFactors;
    //TagsToIgnore = tagsToIgnore;
    TopRankingCount = topRankingCount;
    Smoothing = smoothing;
    NthRoot = nthRoot;
    IntervalHours = intervalHours;
    MinimumDiffQuote = minimumDiffQuote;
    MinimumDiffAllocation = minimumDiffAllocation;
    AutomationEnabled = automationEnabled;
    LastRebalance = lastRebalance;
  }
}