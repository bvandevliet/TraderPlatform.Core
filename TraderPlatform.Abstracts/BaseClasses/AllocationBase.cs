using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.BaseClasses;

public abstract class AllocationBase : ITickerPrice, IPosition
{
  // ABSTRACT OR USING CTOR ??
  public abstract IMarket Market { get; }

  public decimal Price { get; }

  public decimal Amount { get; }

  public decimal AmountAvailable { get; }

  private decimal? amountQuote;
  public decimal AmountQuote
  {
    get => amountQuote ??= Price * Amount;
  }

  private decimal? amountQuoteAvailable;
  public decimal AmountQuoteAvailable
  {
    get => amountQuoteAvailable ??= Price * AmountAvailable;
  }
}