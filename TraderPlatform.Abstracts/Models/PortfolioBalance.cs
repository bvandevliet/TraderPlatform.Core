using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

public class PortfolioBalance : IPortfolioBalance
{
  public IEnumerable<IAsset> Assets { get; set; } = new List<IAsset>();

  public decimal AmountQuoteTotal { get; }
}