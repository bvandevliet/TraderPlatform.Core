namespace TraderPlatform.Abstracts.Interfaces;

public interface IMarketData //: IMarket
{
  //IMarket Market { get; }

  int PricePrecision { get; }

  decimal MinOrderInQuoteAsset { get; }

  decimal MinOrderInBaseAsset { get; }
}