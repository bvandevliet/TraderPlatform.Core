namespace TraderPlatform.Abstracts.Interfaces;

public interface ITickerPrice //: IMarket
{
  IMarket Market { get; }

  decimal Price { get; }
}