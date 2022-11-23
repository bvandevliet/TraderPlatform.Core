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
  /// Total value of balance in quote currency.
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
    get => amountQuoteAvailable ??= GetAllocation(QuoteCurrency)?.AmountQuoteAvailable ?? 0;
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
  /// <param name="asset">The <see cref="IAsset"/> to find allocation of.</param>
  /// <returns></returns>
  public Allocation? GetAllocation(IAsset asset) =>
    allocations.Find(alloc => alloc.Market.BaseCurrency.Equals(asset));

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

    allocation.OnAmountUpdate += ResetAmountQuoteTotal;

    if (allocation.Market.BaseCurrency.Equals(QuoteCurrency))
    {
      allocation.OnAmountAvailableUpdate += ResetAmountQuoteAvailable;
    }

    allocations.Add(allocation);

    ResetAmountQuoteTotal(this);

    if (allocation.Market.BaseCurrency.Equals(QuoteCurrency))
    {
      ResetAmountQuoteAvailable(this);
    }
  }

  /// <summary>
  /// Remove an <see cref="Allocation"/> from the <see cref="Allocations"/> collection.
  /// Note that <see cref="AmountQuoteTotal"/> and <see cref="AmountQuoteAvailable"/> will be reset and related events will be triggered.
  /// </summary>
  /// <param name="asset">The <see cref="IAsset"/> to remove allocation of.</param>
  public void RemoveAllocation(IAsset asset)
  {
    Allocation? allocation = GetAllocation(asset);

    if (allocation != null)
    {
      allocation.OnPriceUpdate -= ResetAmountQuoteTotal;

      allocation.OnAmountUpdate -= ResetAmountQuoteTotal;
      allocation.OnAmountAvailableUpdate -= ResetAmountQuoteAvailable;

      allocations.Remove(allocation);

      ResetAmountQuoteTotal(this);

      if (allocation.Market.BaseCurrency.Equals(QuoteCurrency))
      {
        ResetAmountQuoteAvailable(this);
      }
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