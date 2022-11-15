using System.Collections.ObjectModel;

namespace TraderPlatform.Abstracts.Models;

public class Balance
{
  private readonly List<Allocation> allocations = new();
  public ReadOnlyCollection<Allocation> Allocations { get; }

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

  public Balance()
  {
    Allocations = new(allocations);
  }

  public void AddAllocation(Allocation allocation)
  {
    if (allocations.Any(alloc => alloc.Equals(allocation)))
    {
      return;
    }

    allocations.Add(allocation);

    amountQuoteTotal = null;
    amountQuoteAvailable = null;
  }
}