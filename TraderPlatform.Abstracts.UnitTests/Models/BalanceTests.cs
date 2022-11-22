using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.UnitTests.Models;

[TestClass]
public class BalanceTests
{
  private readonly Asset quoteCurrency = new("EUR");

  /// <inheritdoc/>
  private class BalanceWrapper : Balance
  {
    private bool amountQuoteTotalReset = false;
    private bool amountQuoteAvailableReset = false;

    /// <inheritdoc/>
    public BalanceWrapper(IAsset quoteCurrency) : base(quoteCurrency)
    {
      OnAmountQuoteTotalReset += (sender, e) => amountQuoteTotalReset = true;
      OnAmountQuoteAvailableReset += (sender, e) => amountQuoteAvailableReset = true;
    }

    internal bool AmountQuoteTotalResetEventTriggered() => amountQuoteTotalReset && !(amountQuoteTotalReset = false);
    internal bool AmountQuoteAvailableResetEventTriggered() => amountQuoteAvailableReset && !(amountQuoteAvailableReset = false);
  }

  [TestMethod]
  public void AddAllocation()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, new Asset("ETH")), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsTrue(balance.AmountQuoteAvailableResetEventTriggered());

    // Both allocations should be added.
    Assert.AreEqual(2, balance.Allocations.Count);
  }

  [TestMethod]
  public void RemoveAllocation()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, new Asset("ETH")), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsTrue(balance.AmountQuoteAvailableResetEventTriggered());

    balance.RemoveAllocation(new Market(quoteCurrency, new Asset("BTC")));

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsTrue(balance.AmountQuoteAvailableResetEventTriggered());

    // Allocation should be removed leaving one.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void UpdateAllocation_Price()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 5);

    balance.AddAllocation(alloc);

    // Test amount quote values.
    Assert.AreEqual(0 * 5, balance.AmountQuoteTotal);
    Assert.AreEqual(0 * 5, balance.AmountQuoteAvailable);

    // Reset event states.
    balance.AmountQuoteTotalResetEventTriggered();
    balance.AmountQuoteAvailableResetEventTriggered();

    alloc.Price = 5; // was 0;

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsTrue(balance.AmountQuoteAvailableResetEventTriggered());

    // Test amount quote values.
    Assert.AreEqual(5 * 5, balance.AmountQuoteTotal);
    Assert.AreEqual(5 * 5, balance.AmountQuoteAvailable);
  }

  [TestMethod]
  public void UpdateAllocation_Amount()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 5, 0);

    balance.AddAllocation(alloc);

    // Test amount quote values.
    Assert.AreEqual(5 * 0, balance.AmountQuoteTotal);
    Assert.AreEqual(5 * 0, balance.AmountQuoteAvailable);

    // Reset event states.
    balance.AmountQuoteTotalResetEventTriggered();
    balance.AmountQuoteAvailableResetEventTriggered();

    alloc.Amount = 5; // was 0;

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsFalse(balance.AmountQuoteAvailableResetEventTriggered());

    // Test amount quote values.
    Assert.AreEqual(5 * 5, balance.AmountQuoteTotal);
    Assert.AreEqual(5 * 0, balance.AmountQuoteAvailable);
  }

  [TestMethod]
  public void UpdateAllocation_AmountAvailable()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 5, 5, 0);

    balance.AddAllocation(alloc);

    // Test amount quote values.
    Assert.AreEqual(5 * 5, balance.AmountQuoteTotal);
    Assert.AreEqual(5 * 0, balance.AmountQuoteAvailable);

    // Reset event states.
    balance.AmountQuoteTotalResetEventTriggered();
    balance.AmountQuoteAvailableResetEventTriggered();

    alloc.AmountAvailable = 5; // was 0;

    // Test if events are raised as expected.
    Assert.IsFalse(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsTrue(balance.AmountQuoteAvailableResetEventTriggered());

    // Test amount quote values.
    Assert.AreEqual(5 * 5, balance.AmountQuoteTotal);
    Assert.AreEqual(5 * 5, balance.AmountQuoteAvailable);
  }

  [TestMethod]
  public void AddAllocation_SameReferenceMultipleTimes()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 0);

    balance.AddAllocation(alloc);
    try
    {
      balance.AddAllocation(alloc);
      Assert.Fail();
    }
    catch (Exceptions.ObjectAlreadyExistsException) { }

    // Allocation should only be added once.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_SameMarketReferenceMultipleTimes()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var market = new Market(quoteCurrency, new Asset("BTC"));

    var alloc1 = new Allocation(market, 0, 0);
    var alloc2 = new Allocation(market, 0, 0);

    balance.AddAllocation(alloc1);
    try
    {
      balance.AddAllocation(alloc2);
      Assert.Fail();
    }
    catch (Exceptions.ObjectAlreadyExistsException) { }

    // Allocation should only be added once.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_SameMarketMultipleTimes()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var baseCurrency = new Asset("BTC");

    var alloc1 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);

    balance.AddAllocation(alloc1);
    try
    {
      balance.AddAllocation(alloc2);
      Assert.Fail();
    }
    catch (Exceptions.ObjectAlreadyExistsException) { }

    // Allocation should only be added once.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_WrongQuoteCurrency()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var baseCurrency = new Asset("BTC");

    var alloc1 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);
    var alloc2 = new Allocation(new Market(baseCurrency, quoteCurrency), 0, 0);

    balance.AddAllocation(alloc1);
    try
    {
      balance.AddAllocation(alloc2);
      Assert.Fail();
    }
    catch (Exceptions.InvalidObjectException) { }

    // An allocation against a different quote currency should not be added.
    Assert.AreEqual(1, balance.Allocations.Count);
  }
}