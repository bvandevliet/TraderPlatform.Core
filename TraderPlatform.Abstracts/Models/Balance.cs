using System.Collections.ObjectModel;
using TraderPlatform.Abstracts.EventArgs;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Represents a portfolio balance, containing relative asset allocations.
/// </summary>
public class Balance
{
  /// <summary>
  /// Triggered when <see cref="AmountQuoteTotal"/> has changed.
  /// </summary>
  public event EventHandler? OnAmountQuoteTotalReset;

  /// <summary>
  /// Triggered when <see cref="AmountQuoteAvailable"/> has changed.
  /// </summary>
  public event EventHandler? OnAmountQuoteAvailableReset;

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
  /// Get an <see cref="Allocation"/> for a given <see cref="IMarket"/> if exists.
  /// </summary>
  /// <param name="market">The <see cref="IMarket"/> to find allocation of.</param>
  /// <returns></returns>
  public Allocation? GetAllocation(IMarket market) =>
    allocations.Find(alloc => alloc.Market.Equals(market));

  /// <summary>
  /// Add an <see cref="Allocation"/> to the <see cref="Allocations"/> collection.
  /// Note that <see cref="AmountQuoteTotal"/> and <see cref="AmountQuoteAvailable"/> will be reset and related events will be triggered.
  /// </summary>
  /// <param name="allocation">The <see cref="Allocation"/> to add.</param>
  /// <exception cref="Exceptions.InvalidObjectException"></exception>
  /// <exception cref="Exceptions.ObjectAlreadyExistsException"></exception>
  public void AddAllocation(Allocation allocation)
  {
    if (QuoteCurrency.Symbol != allocation.Market.QuoteCurrency.Symbol)
    {
      throw new Exceptions.InvalidObjectException("Quote currency of given Allocation object does not match with the quote currency of this Balance instance.");
    }

    if (allocations.Any(alloc => alloc.Market.Equals(allocation.Market)))
    {
      throw new Exceptions.ObjectAlreadyExistsException("An allocation in this market already exists.");
    }

    allocation.OnPriceUpdate += ResetAmountQuoteTotal;
    allocation.OnPriceUpdate += ResetAmountQuoteAvailable;

    allocation.OnAmountUpdate += ResetAmountQuoteTotal;
    allocation.OnAmountAvailableUpdate += ResetAmountQuoteAvailable;

    allocations.Add(allocation);

    ResetAmountQuoteTotal(this);
    ResetAmountQuoteAvailable(this);
  }

  /// <summary>
  /// Remove an <see cref="Allocation"/> from the <see cref="Allocations"/> collection.
  /// Note that <see cref="AmountQuoteTotal"/> and <see cref="AmountQuoteAvailable"/> will be reset and related events will be triggered.
  /// </summary>
  /// <param name="market">The <see cref="IMarket"/> to remove allocation of.</param>
  public void RemoveAllocation(IMarket market)
  {
    Allocation? allocation = GetAllocation(market);

    if (allocation != null)
    {
      allocation.OnPriceUpdate -= ResetAmountQuoteTotal;
      allocation.OnPriceUpdate -= ResetAmountQuoteAvailable;

      allocation.OnAmountUpdate -= ResetAmountQuoteTotal;
      allocation.OnAmountAvailableUpdate -= ResetAmountQuoteAvailable;

      allocations.Remove(allocation);

      ResetAmountQuoteTotal(this);
      ResetAmountQuoteAvailable(this);
    }
  }

  private void ResetAmountQuoteTotal(object? sender, NumberUpdateEventArgs? e = null)
  {
    amountQuoteTotal = null;

    OnAmountQuoteTotalReset?.Invoke(this, new());
  }

  private void ResetAmountQuoteAvailable(object? sender, NumberUpdateEventArgs? e = null)
  {
    amountQuoteAvailable = null;

    OnAmountQuoteAvailableReset?.Invoke(this, new());
  }
}