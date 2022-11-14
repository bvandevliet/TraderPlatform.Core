namespace TraderPlatform.Abstracts.Interfaces;

public interface ITickerPrice
{
  IMarket Market { get; }

  decimal Price { get; }
}