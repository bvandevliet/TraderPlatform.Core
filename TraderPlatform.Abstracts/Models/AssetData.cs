namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Additional asset data.
/// </summary>
public class AssetData : Asset
{
  /// <summary>
  /// The precision used for specifiying amounts.
  /// </summary>
  public int Decimals { get; set; }

  /// <summary>
  /// <inheritdoc cref="AssetData"/>
  /// </summary>
  /// <param name="symbol"><inheritdoc cref="Asset.Symbol"/></param>
  /// <param name="name"><inheritdoc cref="Asset.Name"/></param>
  public AssetData(string symbol, string? name = null) : base(symbol, name)
  {
  }
}