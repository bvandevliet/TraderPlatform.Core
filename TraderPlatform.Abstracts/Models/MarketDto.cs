using System.ComponentModel.DataAnnotations;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IMarket"/>
public class MarketDto // : IMarket
{
  /// <inheritdoc cref="IMarket.QuoteCurrency"/>
  [Required]
  public AssetDto QuoteCurrency { get; set; } = null!;

  /// <inheritdoc cref="IMarket.BaseCurrency"/>
  [Required]
  public AssetDto BaseCurrency { get; set; } = null!;
}