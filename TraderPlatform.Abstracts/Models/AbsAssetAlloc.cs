using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

public class AbsAssetAlloc
{
  public IAsset Asset { get; set; }

  public decimal AbsAlloc { get; set; }

  public AbsAssetAlloc(IAsset asset, decimal absAlloc)
  {
    Asset = asset;
    AbsAlloc = absAlloc;
  }

  public AbsAssetAlloc(string baseSymbol, decimal absAlloc)
    : this(new Asset(baseSymbol), absAlloc)
  {
  }
}