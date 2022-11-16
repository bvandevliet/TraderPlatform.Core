using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.UnitTests.Models;

[TestClass]
public class BalanceTests
{
  private readonly Asset quoteCurrency = new("EUR");
  private readonly Asset baseCurrency = new("BTC");

  [TestMethod]
  public void AddAllocation_MultipleTimes()
  {
    var balance = new Balance(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency, baseCurrency), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    /// An allocation should only be added once.
    Assert.AreEqual(2, balance.Allocations.Count);
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