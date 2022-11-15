using System.Collections.ObjectModel;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

public class Balance
{
  /// <summary>
  /// The quote currency on which this balance instance is based.
  /// </summary>
  public IAsset QuoteCurrency { get; }

  private readonly List<Allocation> allocations = new();
  /// <summary>
  /// The collection of <see cref="Allocation"/> instances.
  /// </summary>
  public ReadOnlyCollection<Allocation> Allocations { get; }

  private decimal? amountQuoteTotal;
  /// <summary>
  /// Total value of amount in quote currency.
  /// </summary>
  public decimal AmountQuoteTotal
  {
    get => amountQuoteTotal ??= Allocations.Sum(alloc => alloc.AmountQuote);
  }

  private decimal? amountQuoteAvailable;
  /// <summary>
  /// Total value of available amount in quote currency.
  /// </summary>
  public decimal AmountQuoteAvailable
  {
    get => amountQuoteAvailable ??= Allocations.Sum(alloc => alloc.AmountQuoteAvailable);
  }

  /// <summary>
  /// A collection of <see cref="Allocation"/> instances and total quote amount values.
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="QuoteCurrency"/></param>
  public Balance(IAsset quoteCurrency)
  {
    QuoteCurrency = quoteCurrency;

    Allocations = new(allocations);
  }

  /// <summary>
  /// Add an <see cref="Allocation"/> to the <see cref="Allocations"/> collection.
  /// </summary>
  /// <param name="allocation"></param>
  /// <exception cref="InvalidOperationException"></exception>
  public void AddAllocation(Allocation allocation)
  {
    if (QuoteCurrency.Symbol != allocation.Market.QuoteCurrency.Symbol)
    {
      throw new InvalidOperationException("Quote currency of allocation to be added does not match with the quote currency of this Balance instance.");
    }

    if (allocations.Any(alloc => alloc == allocation))
    {
      return; // Prevent adding a reference to the same allocation multiple times.
    }

    allocations.Add(allocation);

    amountQuoteTotal = null;
    amountQuoteAvailable = null;
  }
}