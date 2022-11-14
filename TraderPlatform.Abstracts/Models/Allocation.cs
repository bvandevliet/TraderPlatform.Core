using TraderPlatform.Abstracts.EventArgs;
using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

public class Allocation : ITickerPrice, IPosition
{
  public event EventHandler<NumberUpdateEventArgs>? PriceUpdate;
  public event EventHandler<NumberUpdateEventArgs>? AmountUpdate;
  public event EventHandler<NumberUpdateEventArgs>? AmountAvailableUpdate;

  public IMarket Market { get; }

  private decimal price;
  public decimal Price
  {
    get => price;
    set
    {
      UpdatePrice(value);
    }
  }

  private decimal amount;
  public decimal Amount
  {
    get => amount;
    set
    {
      UpdateAmount(value);
      UpdateAmountAvailable(Math.Min(value, amountAvailable));
    }
  }

  private decimal amountAvailable;
  public decimal AmountAvailable
  {
    get => amountAvailable;
    set
    {
      UpdateAmountAvailable(Math.Min(value, amount));
    }
  }

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

  public Allocation(
    IMarket market,
    decimal price,
    decimal amount,
    decimal? amountAvailable)
  {
    Market = market;
    this.price = price;
    this.amount = amount;
    this.amountAvailable = Math.Min(amount, amountAvailable ?? amount);
  }

  private void UpdatePrice(decimal newValue)
  {
    amountQuote = null;
    amountQuoteAvailable = null;

    decimal oldValue = price;
    price = newValue;

    if (oldValue != newValue)
      PriceUpdate?.Invoke(this, new(oldValue, newValue));
  }

  private void UpdateAmount(decimal newValue)
  {
    amountQuote = null;

    decimal oldValue = amount;
    amount = newValue;

    if (oldValue != newValue)
      AmountUpdate?.Invoke(this, new(amount, newValue));
  }

  private void UpdateAmountAvailable(decimal newValue)
  {
    amountQuoteAvailable = null;

    decimal oldValue = amountAvailable;
    amountAvailable = newValue;

    if (oldValue != newValue)
      AmountAvailableUpdate?.Invoke(this, new(amountAvailable, newValue));
  }
}