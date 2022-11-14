namespace TraderPlatform.Abstracts.Interfaces;

public interface IMarketData : IMarket
{
  int PricePrecision { get; }

  decimal MinOrderInQuoteAsset { get; }

  decimal MinOrderInBaseAsset { get; }
}