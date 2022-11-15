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
      UpdateAmountAvailable(Math.Min(value, amountAvailable));
      UpdateAmount(value);
    }
  }

  private decimal amountAvailable;
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