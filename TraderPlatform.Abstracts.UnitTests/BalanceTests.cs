using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.UnitTests;

[TestClass]
public class BalanceTests
{
  [TestMethod]
  public void AddAllocation_MultipleTimes()
  {
    var quoteCurrency = new Asset("EUR");

    var balance = new Balance(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency), 0, 0);
    var alloc2 = new Allocation(new Market(quoteCurrency), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    /// An allocation should only be added once.
    Assert.AreEqual(2, balance.Allocations.Count);
  }

  [TestMethod]
  public void AddAllocation_WrongQuoteCurrency()
  {
    var quoteCurrency = new Asset("EUR");

    var balance = new Balance(quoteCurrency);

    var alloc1 = new Allocation(new Market(quoteCurrency), 0, 0);
    var alloc2 = new Allocation(new Market(new Asset("BTC")), 0, 0);

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