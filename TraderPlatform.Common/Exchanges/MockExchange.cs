using TraderPlatform.Abstracts.Enums;
using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;

namespace TraderPlatform.Common.Exchanges;

/// <inheritdoc cref="IExchangeService"/>
public class MockExchange : IExchangeService
{
  protected Balance curBalance = null!;

  /// <inheritdoc/>
  public Asset QuoteCurrency { get; protected set; } = new("EUR");

  /// <inheritdoc/>
  public decimal MinimumOrderSize { get; protected set; }

  /// <inheritdoc/>
  public decimal MakerFee { get; protected set; }

  /// <inheritdoc/>
  public decimal TakerFee { get; protected set; }

  protected void Init(Balance? curBalance)
  {
    if (null != curBalance)
    {
      this.curBalance = curBalance;
    }
    else
    {
      decimal deposit = 1000;

      this.curBalance = new(QuoteCurrency);

      this.curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("EUR")), 000001, .05m * deposit));
      this.curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("BTC")), 18_000, .40m * deposit / 15_000));
      this.curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("ETH")), 01_610, .30m * deposit / 01_400));
      this.curBalance.AddAllocation(new(new Market(QuoteCurrency, new Asset("BNB")), 000306, .25m * deposit / 000340));
      //                                                                                     100%
    }
  }

  /// <summary>
  /// <inheritdoc cref="IExchangeService"/>
  /// </summary>
  /// <param name="curBalance"><inheritdoc cref="Balance"/></param>
  public MockExchange(Balance? curBalance = null)
  {
    Init(curBalance);
  }

  /// <summary>
  /// <inheritdoc cref="IExchangeService"/>
  /// </summary>
  /// <param name="quoteCurrency"><inheritdoc cref="QuoteCurrency"/></param>
  /// <param name="minimumOrderSize"><inheritdoc cref="MinimumOrderSize"/></param>
  /// <param name="makerFee"><inheritdoc cref="MakerFee"/></param>
  /// <param name="takerFee"><inheritdoc cref="TakerFee"/></param>
  /// <param name="curBalance"><inheritdoc cref="Balance"/></param>
  public MockExchange(
    Asset quoteCurrency,
    decimal minimumOrderSize,
    decimal makerFee,
    decimal takerFee,
    Balance? curBalance = null)
  {
    QuoteCurrency = quoteCurrency;
    MinimumOrderSize = minimumOrderSize;
    MakerFee = makerFee;
    TakerFee = takerFee;

    Init(curBalance);
  }

  /// <summary>
  /// <inheritdoc cref="IExchangeService"/>
  /// </summary>
  /// <param name="exchangeService">Instance of the exchange service to base this mock instance on.</param>
  /// <param name="curBalance"><inheritdoc cref="Balance"/></param>
  public MockExchange(IExchangeService exchangeService, Balance? curBalance = null)
    : this(
      exchangeService.QuoteCurrency,
      exchangeService.MinimumOrderSize,
      exchangeService.MakerFee,
      exchangeService.TakerFee,
      // Override current balance with the actual one from the underlying exchange service if none given.
      curBalance ?? exchangeService.GetBalance().Result)
  {
  }

  /// <summary>
  /// If no initial <see cref="Balance"/> was given,
  /// this returns a drifted <see cref="Balance"/> that had a initial value of €1000 and initially allocated as follows:
  /// <br/>
  /// EUR :  5 %
  /// <br/>
  /// BTC : 40 %
  /// <br/>
  /// ETH : 30 %
  /// <br/>
  /// BNB : 25 %
  /// </summary>
  /// <returns></returns>
  public Task<Balance> GetBalance()
  {
    return Task.FromResult(curBalance);
  }

  /// <inheritdoc/>
  public Task<object> DepositHistory()
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<object> WithdrawHistory()
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<object> GetCandles(Market market, CandleInterval interval, int limit)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<bool> IsTradable(Market market)
  {
    return Task.FromResult(true);
  }

  /// <inheritdoc/>
  public Task<ITickerPrice> GetPrice(Market market)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder> NewOrder(IOrder order)
  {
    Allocation? curAlloc = curBalance.GetAllocation(order.Market.BaseCurrency);

    Allocation newAlloc = curAlloc ?? new(order.Market, order.Price, order.Amount);

    Allocation? quoteAlloc = curBalance.GetAllocation(QuoteCurrency);

    Allocation newQuoteAlloc = quoteAlloc ?? new(new Market(QuoteCurrency, QuoteCurrency), 1);

    if (order.Side == OrderSide.Buy)
    {
      newAlloc.AmountQuote += order.AmountQuote;

      newQuoteAlloc.AmountQuote -= order.AmountQuote;
    }
    else
    {
      newAlloc.AmountQuote -= order.AmountQuote;

      newQuoteAlloc.AmountQuote += order.AmountQuote;
    }

    order.Status = OrderStatus.Filled;

    if (null == curAlloc)
    {
      curBalance.AddAllocation(newAlloc);
    }

    if (null == quoteAlloc)
    {
      curBalance.AddAllocation(newQuoteAlloc);
    }

    return Task.FromResult(order);
  }

  /// <inheritdoc/>
  public Task<IOrder> NewOrder(Market market, OrderSide side, OrderType type, IOrderArgs orderArgs)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder?> GetOrder(string orderId, Market? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IOrder?> CancelOrder(string orderId, Market? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> GetOpenOrders(Market? market = null)
  {
    throw new NotImplementedException();
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> CancelAllOpenOrders(Market? market = null)
  {
    return Task.FromResult((IEnumerable<IOrder>)new List<Order>());
  }

  /// <inheritdoc/>
  public Task<IEnumerable<IOrder>> SellAllPositions(Asset? asset = null)
  {
    throw new NotImplementedException();
  }
}

/// <inheritdoc cref="MockExchange"/>
public class MockExchange<T> : MockExchange, IExchangeService where T : class, IExchangeService
{
  /// <summary>
  /// The underlying exchange service instance this mock instance is based on.
  /// </summary>
  public T ExchangeService { get; }

  /// <summary>
  /// <inheritdoc cref="IExchangeService"/>
  /// </summary>
  public MockExchange(T exchangeService) : base()
  {
    ExchangeService = exchangeService;

    QuoteCurrency = ExchangeService.QuoteCurrency;
    MinimumOrderSize = ExchangeService.MinimumOrderSize;
    MakerFee = ExchangeService.MakerFee;
    TakerFee = ExchangeService.TakerFee;

    // Override current balance with the actual one from the underlying exchange service.
    try
    {
      curBalance = ExchangeService.GetBalance().Result;
    }
    catch (NotImplementedException)
    {
      //Init(null);
    }
  }
}