using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.UnitTests.Models;

[TestClass]
public class BalanceTests
{
  private readonly Asset quoteCurrency = new("EUR");
  private readonly Asset baseCurrency = new("BTC");

  [TestMethod]
  public void AddAllocation()
  {
    var balance = new Balance(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, new Asset("ETH")), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    // Both allocations should be added.
    Assert.AreEqual(2, balance.Allocations.Count);
  }

  [TestMethod]
  public void RemoveAllocation()
  {
    var balance = new Balance(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency, new Asset("BTC")), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, new Asset("ETH")), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    balance.RemoveAllocation(new Market(quoteCurrency, new Asset("BTC")));

    // Allocation should be removed leaving one.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_SameReferenceMultipleTimes()
  {
    var balance = new Balance(quoteCurrency);

    var alloc = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);

    balance.AddAllocation(alloc);
    try
    {
      balance.AddAllocation(alloc);
      Assert.Fail();
    }
    catch (InvalidOperationException) { }

    // Allocation should only be added once.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_SameMarketReferenceMultipleTimes()
  {
    var balance = new Balance(quoteCurrency);

    var market = new Market(quoteCurrency, baseCurrency);

    var alloc1 = new Allocation(market, 0, 0);
    var alloc2 = new Allocation(market, 0, 0);

    balance.AddAllocation(alloc1);
    try
    {
      balance.AddAllocation(alloc2);
      Assert.Fail();
    }
    catch (InvalidOperationException) { }

    // Allocation should only be added once.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_SameMarketMultipleTimes()
  {
    var balance = new Balance(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);

    balance.AddAllocation(alloc1);
    try
    {
      balance.AddAllocation(alloc2);
      Assert.Fail();
    }
    catch (InvalidOperationException) { }

    // Allocation should only be added once.
    Assert.AreEqual(1, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_WrongQuoteCurrency()
  {
    var balance = new Balance(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);
    var alloc2 = new Allocation(new Market(baseCurrency, quoteCurrency), 0, 0);

    balance.AddAllocation(alloc1);
    try
    {
      balance.AddAllocation(alloc2);
      Assert.Fail();
    }
    catch (InvalidOperationException) { }

    // An allocation against a different quote currency should not be added.
    Assert.AreEqual(1, balance.Allocations.Count);
  }
}