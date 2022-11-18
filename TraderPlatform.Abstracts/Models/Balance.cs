using System.Collections.ObjectModel;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Represents a portfolio balance, containing relative asset allocations.
/// </summary>
public class Balance
{
  /// <summary>
  /// The quote currency on which this balance instance is based.
  /// </summary>
  public IAsset QuoteCurrency { get; }

  private readonly List<Allocation> allocations = new();
  /// <summary>
  /// Collection of <see cref="Allocation"/> instances.
  /// </summary>
  public ReadOnlyCollection<Allocation> Allocations { get; }

  private decimal? amountQuoteTotal;
  /// <summary>
  /// Total amount in quote currency.
  /// </summary>
  public decimal AmountQuoteTotal
  {
    get => amountQuoteTotal ??= Allocations.Sum(alloc => alloc.AmountQuote);
  }

  private decimal? amountQuoteAvailable;
  /// <summary>
  /// Total freely available amount in quote currency.
  /// </summary>
  public decimal AmountQuoteAvailable
  {
    get => amountQuoteAvailable ??= Allocations.Sum(alloc => alloc.AmountQuoteAvailable);
  }

  /// <summary>
  /// Collection of <see cref="Allocation"/> instances and total quote amount values.
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="QuoteCurrency"/></param>
  public Balance(IAsset quoteCurrency)
  {
    QuoteCurrency = quoteCurrency;

    Allocations = allocations.AsReadOnly();
  }

  /// <summary>
  /// Add an <see cref="Allocation"/> to the <see cref="Allocations"/> collection.
  /// </summary>
  /// <param name="allocation">The <see cref="Allocation"/> to add.</param>
  /// <exception cref="InvalidOperationException"></exception>
  public void AddAllocation(Allocation allocation)
  {
    if (QuoteCurrency.Symbol != allocation.Market.QuoteCurrency.Symbol)
    {
      throw new InvalidOperationException("Quote currency of allocation does not match with the quote currency of this Balance instance.");
    }

    if (allocations.Any(alloc => alloc.Market.Equals(allocation.Market)))
    {
      throw new InvalidOperationException("An allocation in this market already exists.");
    }

    allocations.Add(allocation);

    amountQuoteTotal = null;
    amountQuoteAvailable = null;
  }

  /// <summary>
  /// Remove an <see cref="Allocation"/> from the <see cref="Allocations"/> collection.
  /// </summary>
  /// <param name="market">The <see cref="IMarket"/> to remove allocation of.</param>
  /// <returns>If the <see cref="Allocation"/> was removed. If false, the <see cref="Allocation"/> didn't exist.</returns>
  public bool RemoveAllocation(Market market)
  {
    if (allocations.RemoveAll(alloc => alloc.Market.Equals(market)) > 0)
    {
      amountQuoteTotal = null;
      amountQuoteAvailable = null;

      return true;
    }

    return false;
  }
}