using TraderPlatform.Abstracts.EventArgs;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <summary>
/// Represents an asset allocation.
/// </summary>
public class Allocation : ITickerPrice, IPosition
{
  /// <summary>
  /// Triggered when <see cref="Price"/> has changed.
  /// Note that <see cref="amountQuote"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? OnPriceUpdate;

  /// <summary>
  /// Triggered when <see cref="Amount"/> has changed.
  /// Note that <see cref="amountQuote"/> will also become outdated.
  /// </summary>
  public event EventHandler<NumberUpdateEventArgs>? OnAmountUpdate;

  /// <summary>
  /// The market this instance represents an allocation in.
  /// </summary>
  public IMarket Market { get; }

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
  /// <inheritdoc cref="IPosition.Amount"/>
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
  }

  /// <summary>
  /// Represents an allocation in a given <see cref="IAsset"/> against a given quote currency as defined by <paramref name="market"/>.
  /// </summary>
  /// <param name="market"><inheritdoc cref="Market"/></param>
  /// <param name="price"><inheritdoc cref="Price"/></param>
  /// <param name="amount"><inheritdoc cref="Amount"/></param>
  public Allocation(
    IMarket market,
    decimal price,
    decimal amount)
  {
    Market = market;
    this.price = price;
    this.amount = amount;
  }

  private void UpdatePrice(decimal newValue)
  {
    decimal oldValue = price;
    price = newValue;

    if (oldValue != newValue)
    {
      amountQuote = null;

      OnPriceUpdate?.Invoke(this, new(oldValue, newValue));
    }
  }

  private void UpdateAmount(decimal newValue)
  {
    decimal oldValue = amount;
    amount = newValue;

    if (oldValue != newValue)
    {
      amountQuote = null;

      OnAmountUpdate?.Invoke(this, new(amount, newValue));
    }
  }
}