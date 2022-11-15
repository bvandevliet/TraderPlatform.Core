using TraderPlatform.Abstracts.EventArgs;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

public class Allocation : ITickerPrice, IPosition
{
  /// <summary>
  /// Triggered when <see cref="Price"/> has changed.
  /// Note that <see cref="amountQuote"/> and <see cref="amountQuoteAvailable"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? PriceUpdate;

  /// <summary>
  /// Triggered when <see cref="Amount"/> has changed.
  /// Note that <see cref="amountQuote"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? AmountUpdate;

  /// <summary>
  /// Triggered when <see cref="AmountAvailable"/> has changed.
  /// Note that <see cref="amountQuoteAvailable"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? AmountAvailableUpdate;

  /// <summary>
  /// The market this instance represents an allocation in.
  /// </summary>
  public IMarket Market { get; }

  private decimal price;
  /// <summary>
  /// Price per unit in quote currency as specified in <see cref="Market"/>.
  /// </summary>
  public decimal Price
  {
    get => price;
    set
    {
      UpdatePrice(value);
    }
  }

  private decimal amount;
  /// <inheritdoc cref="IPosition.Amount"/>
  public decimal Amount
  {
    get => amount;
    set
    {
      UpdateAmountAvailable(Math.Min(value, amountAvailable));
      UpdateAmount(value);
    }
  }

  private decimal amountAvailable;
  /// <inheritdoc cref="IPosition.AmountAvailable"/>
  public decimal AmountAvailable
  {
    get => amountAvailable;
    set
    {
      UpdateAmount(Math.Max(value, amount));
      UpdateAmountAvailable(value);
    }
  }

  private decimal? amountQuote;
  /// <summary>
  /// Value of amount in quote currency.
  /// </summary>
  public decimal AmountQuote
  {
    get => amountQuote ??= Price * Amount;
  }

  private decimal? amountQuoteAvailable;
  /// <summary>
  /// Value of available amount in quote currency.
  /// </summary>
  public decimal AmountQuoteAvailable
  {
    get => amountQuoteAvailable ??= Price * AmountAvailable;
  }

  /// <summary>
  /// Represents an allocation in a given <see cref="IAsset"/> against a given quote currency as defined by <paramref name="market"/>.
  /// </summary>
  /// <param name="market"><inheritdoc cref="Market"/></param>
  /// <param name="price"><inheritdoc cref="Price"/></param>
  /// <param name="amount"><inheritdoc cref="Amount"/></param>
  /// <param name="amountAvailable"><inheritdoc cref="AmountAvailable"/></param>
  public Allocation(
    IMarket market,
    decimal price,
    decimal amount,
    decimal? amountAvailable = null)
  {
    Market = market;
    this.price = price;
    this.amount = amount;
    this.amountAvailable = Math.Min(amount, amountAvailable ?? amount);
  }

  private void UpdatePrice(decimal newValue)
  {
    decimal oldValue = price;
    price = newValue;

    if (oldValue != newValue)
    {
      amountQuote = null;
      amountQuoteAvailable = null;

      PriceUpdate?.Invoke(this, new(oldValue, newValue));
    }
  }

  private void UpdateAmount(decimal newValue)
  {
    decimal oldValue = amount;
    amount = newValue;

    if (oldValue != newValue)
    {
      amountQuote = null;

      AmountUpdate?.Invoke(this, new(amount, newValue));
    }
  }

  private void UpdateAmountAvailable(decimal newValue)
  {
    decimal oldValue = amountAvailable;
    amountAvailable = newValue;

    if (oldValue != newValue)
    {
      amountQuoteAvailable = null;

      AmountAvailableUpdate?.Invoke(this, new(amountAvailable, newValue));
    }
  }
}