namespace TraderPlatform.Abstracts.Interfaces;

public interface IPortfolioBalance
{
  IEnumerable<IAsset> Assets { get; set; }

  decimal AmountQuoteTotal { get; }
}