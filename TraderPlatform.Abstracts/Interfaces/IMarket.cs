namespace TraderPlatform.Abstracts.Interfaces;

public interface IMarket
{
  IAsset QuoteCurrency { get; set; }

  IAsset BaseCurrency { get; set; }
}