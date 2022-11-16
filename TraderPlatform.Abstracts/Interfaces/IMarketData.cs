namespace TraderPlatform.Abstracts.Interfaces;

public interface IMarketData : IMarket
{
  int PricePrecision { get; set; }

  decimal MinOrderInQuoteCurrency { get; set; }

  decimal MinOrderInBaseCurrency { get; set; }
}