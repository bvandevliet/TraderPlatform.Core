using TraderPlatform.Abstracts.EventArgs;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Represents an asset allocation.
/// </summary>
public class Allocation : ITickerPrice
{
  /// <summary>
  /// Triggered when <see cref="Price"/> has changed.
  /// Note that <see cref="AmountQuote"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? OnPriceUpdate;

  /// <summary>
  /// Triggered when <see cref="Amount"/> has changed.
  /// Note that <see cref="AmountQuote"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? OnAmountUpdate;

  /// <summary>
  /// Triggered when <see cref="AmountQuote"/> has changed.
  /// Note that <see cref="Amount"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? OnAmountQuoteUpdate;

  /// <summary>
  /// The market this instance represents an allocation in.
  /// </summary>
  public Market Market { get; }

  private decimal price;
  /// <inheritdoc cref="ITickerPrice.Price"/>
  public decimal Price
  {
    get => price;
    set
    {
      UpdatePrice(value);
    }
  }

  private decimal amount;
  /// <summary>
  /// Amount in base currency.
  /// </summary>
  public decimal Amount
  {
    get => amount;
    set
    {
      UpdateAmount(value);
    }
  }

  private decimal? amountQuote;
  /// <summary>
  /// Amount in quote currency.
  /// </summary>
  public decimal AmountQuote
  {
    get => amountQuote ??= Price * Amount;
    set
    {
      UpdateAmountQuote(value);
    }
  }

  /// <summary>
  /// Represents an allocation in a given <see cref="Asset"/> against a given quote currency as defined by <paramref name="market"/>.
  /// </summary>
  /// <param name="market"><inheritdoc cref="Market"/></param>
  /// <param name="price"><inheritdoc cref="Price"/></param>
  /// <param name="amount"><inheritdoc cref="Amount"/></param>
  public Allocation(
    Market market,
    decimal? price = null,
    decimal? amount = null)
  {
    Market = market;
    this.price = price ?? 0;
    this.amount = amount ?? 0;
  }

  /// <summary>
  /// <inheritdoc cref="Allocation(Market, decimal?, decimal?)"/>
  /// </summary>
  /// <param name="quoteSymbol"><inheritdoc cref="Market.QuoteCurrency"/></param>
  /// <param name="baseSymbol"><inheritdoc cref="Market.BaseCurrency"/></param>
  /// <param name="price"><inheritdoc cref="Price"/></param>
  /// <param name="amount"><inheritdoc cref="Amount"/></param>
  public Allocation(
    string quoteSymbol,
    string baseSymbol,
    decimal? price = null,
    decimal? amount = null)
    : this(new Market(new Asset(quoteSymbol), new Asset(baseSymbol)), price, amount)
  {
  }

  private void UpdatePrice(decimal newValue)
  {
    decimal oldValue = Price;
    price = newValue;

    if (oldValue != newValue)
    {
      amountQuote = null;

      OnPriceUpdate?.Invoke(this, new(oldValue, newValue));
    }
  }

  private void UpdateAmount(decimal newValue)
  {
    decimal oldValue = Amount;
    amount = newValue;

    if (oldValue != newValue)
    {
      amountQuote = null;

      OnAmountUpdate?.Invoke(this, new(oldValue, newValue));
    }
  }

  private void UpdateAmountQuote(decimal newValue)
  {
    decimal oldValue = AmountQuote;
    amountQuote = newValue;

    if (oldValue != newValue)
    {
      //price *= oldValue == 0 ? 1 : newValue / oldValue;

      amount = price == 0 ? 0 : newValue / price;

      OnAmountQuoteUpdate?.Invoke(this, new(oldValue, newValue));
    }
  }
}