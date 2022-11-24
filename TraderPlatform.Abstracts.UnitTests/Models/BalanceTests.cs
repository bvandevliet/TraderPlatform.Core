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

    var alloc0 = new Allocation(new Market(quoteCurrency, quoteCurrency), 1, 10);
    var alloc1 = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, new Asset("ETH")), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsFalse(balance.AmountQuoteAvailableResetEventTriggered());

    balance.AddAllocation(alloc0);

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsTrue(balance.AmountQuoteAvailableResetEventTriggered());

    // Both allocations should be added.
    Assert.AreEqual(3, balance.Allocations.Count);
  }

  [TestMethod]
  public void RemoveAllocation()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc0 = new Allocation(new Market(quoteCurrency, quoteCurrency), 1, 10);
    var alloc1 = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, new Asset("ETH")), 0, 0);

    balance.AddAllocation(alloc0);
    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    // Reset event states.
    balance.AmountQuoteTotalResetEventTriggered();
    balance.AmountQuoteAvailableResetEventTriggered();

    balance.RemoveAllocation(new Asset("BTC"));

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsFalse(balance.AmountQuoteAvailableResetEventTriggered());

    balance.RemoveAllocation(quoteCurrency);

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

    // Test amount quote value.
    Assert.AreEqual(0 * 5, balance.AmountQuoteTotal);

    // Reset event states.
    balance.AmountQuoteTotalResetEventTriggered();
    balance.AmountQuoteAvailableResetEventTriggered();

    alloc.Price = 5; // was 0;

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsFalse(balance.AmountQuoteAvailableResetEventTriggered());

    // Test amount quote value.
    Assert.AreEqual(5 * 5, balance.AmountQuoteTotal);
  }

  [TestMethod]
  public void UpdateAllocation_Amount()
  {
    var balance = new BalanceWrapper(quoteCurrency);

    var alloc = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 5, 0);

    balance.AddAllocation(alloc);

    // Test amount quote value.
    Assert.AreEqual(5 * 0, balance.AmountQuoteTotal);

    // Reset event states.
    balance.AmountQuoteTotalResetEventTriggered();
    balance.AmountQuoteAvailableResetEventTriggered();

    alloc.Amount = 5; // was 0;

    // Test if events are raised as expected.
    Assert.IsTrue(balance.AmountQuoteTotalResetEventTriggered());
    Assert.IsFalse(balance.AmountQuoteAvailableResetEventTriggered());

    // Test amount quote value.
    Assert.AreEqual(5 * 5, balance.AmountQuoteTotal);
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