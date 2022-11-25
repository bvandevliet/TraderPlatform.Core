using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Engine.Models;

public class AbsAssetAlloc
{
  public IAsset Asset { get; set; }

  public decimal AbsAlloc { get; set; }

  public AbsAssetAlloc(IAsset asset, decimal absAlloc)
  {
    Asset = asset;
    AbsAlloc = absAlloc;
  }
}