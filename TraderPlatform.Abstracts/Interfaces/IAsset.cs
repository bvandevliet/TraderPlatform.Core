namespace TraderPlatform.Abstracts.Interfaces;

public interface IAsset
{
  string Symbol { get; set; }

  decimal Price { get; set; }

  decimal Amount { get; set; }

  decimal AmountAvailable { get; set; }

  decimal AmountQuote { get; }

  decimal AmountQuoteAvailable { get; }
}