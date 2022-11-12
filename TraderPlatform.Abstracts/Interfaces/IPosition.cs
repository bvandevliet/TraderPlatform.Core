namespace TraderPlatform.Abstracts.Interfaces;

public interface IPosition //: IAsset
{
  //IAsset Asset { get; }

  decimal Amount { get; }

  decimal AmountAvailable { get; }
}