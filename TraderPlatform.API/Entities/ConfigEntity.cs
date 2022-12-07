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
  [Required]
  [ForeignKey("QuoteCurrencyId")]
  public AssetEntity QuoteCurrency { get; set; }
  public int QuoteCurrencyId { get; set; }

  /// <inheritdoc cref="IConfig.QuoteAllocation"/>
  [Required]
  public decimal QuoteAllocation { get; set; }

  /// <inheritdoc cref="IConfig.AltWeightingFactors"/>
  [Required]
  public Dictionary<AssetEntity, decimal> AltWeightingFactors { get; set; }

  /// <inheritdoc cref="IConfig.TagsToIgnore"/>
  [Required]
  public List<string> TagsToIgnore { get; set; }

  /// <inheritdoc cref="IConfig.TopRankingCount"/>
  [Required]
  public int TopRankingCount { get; set; }

  /// <inheritdoc cref="IConfig.Smoothing"/>
  [Required]
  public decimal Smoothing { get; set; }

  /// <inheritdoc cref="IConfig.NthRoot"/>
  [Required]
  public decimal NthRoot { get; set; }

  /// <inheritdoc cref="IConfig.IntervalHours"/>
  [Required]
  public decimal IntervalHours { get; set; }

  /// <inheritdoc cref="IConfig.MinimumDiffQuote"/>
  [Required]
  public decimal MinimumDiffQuote { get; set; }

  /// <inheritdoc cref="IConfig.MinimumDiffAllocation"/>
  [Required]
  public decimal MinimumDiffAllocation { get; set; }

  /// <inheritdoc cref="IConfig.AutomationEnabled"/>
  [Required]
  public bool AutomationEnabled { get; set; }

  /// <inheritdoc cref="IConfig.LastRebalance"/>
  public DateTime? LastRebalance { get; set; }

  public ConfigEntity(
    AssetEntity quoteCurrency,
    decimal quoteAllocation,
    Dictionary<AssetEntity, decimal> altWeightingFactors,
    List<string> tagsToIgnore,
    int topRankingCount,
    decimal smoothing,
    decimal nthRoot,
    decimal intervalHours,
    decimal minimumDiffQuote,
    decimal minimumDiffAllocation,
    bool automationEnabled,
    DateTime? lastRebalance = null)
  {
    QuoteCurrency = quoteCurrency;
    QuoteAllocation = quoteAllocation;
    AltWeightingFactors = altWeightingFactors;
    TagsToIgnore = tagsToIgnore;
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