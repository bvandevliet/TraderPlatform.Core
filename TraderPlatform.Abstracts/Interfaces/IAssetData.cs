namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Additional asset data.
/// </summary>
public interface IAssetData : IAsset
{
  /// <summary>
  /// The precision used for specifiying amounts.
  /// </summary>
  int Decimals { get; set; }
}