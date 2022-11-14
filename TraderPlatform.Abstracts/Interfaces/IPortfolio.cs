namespace TraderPlatform.Abstracts.Interfaces;

public interface IPortfolio
{
  Dictionary<IAsset, IPosition> Positions { get; }
}