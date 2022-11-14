namespace TraderPlatform.Abstracts.Interfaces;

public interface IAssetData : IAsset
{
  int Decimals { get; }
}