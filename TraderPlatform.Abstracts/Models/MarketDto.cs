using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="Market"/>
public class MarketDto // : Market
{
  /// <inheritdoc cref="Market.QuoteCurrency"/>
  [Required]
  public AssetDto QuoteCurrency { get; set; } = null!;

  /// <inheritdoc cref="Market.BaseCurrency"/>
  [Required]
  public AssetDto BaseCurrency { get; set; } = null!;
}