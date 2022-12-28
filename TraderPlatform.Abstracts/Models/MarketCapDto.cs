using System.ComponentModel.DataAnnotations;

namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Market cap data.
/// </summary>
public class MarketCapDto
{
  /// <summary>
  /// Timestamp of the last time this asset's market data was updated.
  /// </summary>
  public DateTime Updated { get; set; }

  /// <summary>
  /// Market in which the market cap is calculated.
  /// </summary>
  [Required]
  public MarketDto Market { get; set; } = null!;

  /// <summary>
  /// Price in quote currency per unit of base currency.
  /// </summary>
  public double Price { get; set; }

  /// <summary>
  /// Market cap in the specified quote currency.
  /// </summary>
  public double MarketCap { get; set; }
}