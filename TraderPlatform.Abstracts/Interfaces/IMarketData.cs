namespace TraderPlatform.Abstracts.Interfaces;

public interface IMarketData : IMarket
{
  int PricePrecision { get; }

  decimal MinOrderInQuoteCurrency { get; }

  decimal MinOrderInBaseCurrency { get; }
}