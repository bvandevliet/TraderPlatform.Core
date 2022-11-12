namespace TraderPlatform.Abstracts.BaseClasses;

public abstract class BalanceBase
{
  // ABSTRACT OR USING CTOR ??
  public abstract IEnumerable<AllocationBase> Allocations { get; }

  private decimal? amountQuoteTotal;
  public decimal AmountQuoteTotal
  {
    get => amountQuoteTotal ??= Allocations.Sum(alloc => alloc.AmountQuote);
  }

  private decimal? amountQuoteAvailable;
  public decimal AmountQuoteAvailable
  {
    get => amountQuoteAvailable ??= Allocations.Sum(alloc => alloc.AmountQuoteAvailable);
  }
}