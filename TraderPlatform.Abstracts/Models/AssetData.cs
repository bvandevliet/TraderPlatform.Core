using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IAssetData"/>
public class AssetData : Asset, IAssetData
{
  /// <summary>
  /// <inheritdoc cref="IAssetData"/>
  /// </summary>
  /// <param name="symbol"><inheritdoc cref="Asset.Symbol"/></param>
  /// <param name="name"><inheritdoc cref="Asset.Name"/></param>
  public AssetData(string symbol, string? name = null) : base(symbol, name)
  {
  }

  /// <inheritdoc/>
  public int Decimals { get; set; }
}