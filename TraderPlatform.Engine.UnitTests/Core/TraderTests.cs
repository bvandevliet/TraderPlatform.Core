using TraderPlatform.Abstracts.Models;
using TraderPlatform.Engine.Models;

namespace TraderPlatform.Engine.Core.Tests;

[TestClass()]
public class TraderTests
{
  [TestMethod()]
  public void GetAllocationQuoteDiffsTest()
  {
    Asset quoteCurrency = new("EUR");

    decimal deposit = 1000;

    Balance curBalance = new(quoteCurrency);
    curBalance.AddAllocation(new(new Market(quoteCurrency, new Asset("BTC")), 18_000, .45m * deposit / 15_000));
    curBalance.AddAllocation(new(new Market(quoteCurrency, new Asset("ETH")), 01_610, .30m * deposit / 01_400));
    curBalance.AddAllocation(new(new Market(quoteCurrency, new Asset("BNB")), 00306m, .25m * deposit / 000340));

    List<AbsAssetAlloc> absAssetAlloc = new()
    {
      new(new Asset("BTC"), .45m),
      new(new Asset("ETH"), .30m),
      new(new Asset("ADA"), .25m),
    };

    var quoteDiffs = Trader.GetAllocationQuoteDiffs(absAssetAlloc, curBalance).ToList();

    Assert.AreEqual(4, quoteDiffs.Count);

    Assert.AreEqual(0040.5, (double)Math.Round(quoteDiffs[0].Value, 1));
    Assert.AreEqual(0012.0, (double)Math.Round(quoteDiffs[1].Value, 1));
    Assert.AreEqual(0225.0, (double)Math.Round(quoteDiffs[2].Value, 1));
    Assert.AreEqual(-277.5, (double)Math.Round(quoteDiffs[3].Value, 1));
  }
}