using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.UnitTests;

[TestClass]
public class BalanceTests
{
  [TestMethod]
  public void AddAllocation()
  {
    var balance = new Balance();

    var alloc1 = new Allocation(new Market(), 0, 0);
    var alloc2 = new Allocation(new Market(), 0, 0);

    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc1);
    balance.AddAllocation(alloc2);

    Assert.AreEqual(2, balance.Allocations.Count);
  }
}