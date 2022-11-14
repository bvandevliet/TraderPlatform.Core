namespace TraderPlatform.Abstracts.Interfaces;

public interface IMarket
{
  IAsset QuoteCurrency { get; }

  IAsset BaseCurrency { get; }
}