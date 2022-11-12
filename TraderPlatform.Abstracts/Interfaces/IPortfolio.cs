namespace TraderPlatform.Abstracts.Interfaces;

public interface IPortfolio
{
  // OR USE INTERFACE ??
  IEnumerable<KeyValuePair<IAsset, IPosition>> Positions { get; set; }

  //IEnumerable<IAssetPosition> Positions { get; set; }
}

//public interface IAssetPosition : IAsset, IPosition { }