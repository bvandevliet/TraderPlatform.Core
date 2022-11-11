using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

public class Asset : IAsset
{
  public string Symbol { get; set; } = string.Empty;

  public decimal Price { get; set; }

  public decimal Amount { get; set; }

  public decimal AmountAvailable { get; set; }

  private decimal? amountQuote;
  public decimal AmountQuote
  {
    get => this.amountQuote ??= Price * Amount;
  }

  private decimal? amountQuoteAvailable;
  public decimal AmountQuoteAvailable
  {
    get => this.amountQuoteAvailable ??= Price * AmountAvailable;
  }
}