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
    curBalance.AddAllocation(new(new Market(quoteCurrency, new Asset("EUR")), 000001, .05m * deposit));
    curBalance.AddAllocation(new(new Market(quoteCurrency, new Asset("BTC")), 18_000, .40m * deposit / 15_000));
    curBalance.AddAllocation(new(new Market(quoteCurrency, new Asset("ETH")), 01_610, .30m * deposit / 01_400));
    curBalance.AddAllocation(new(new Market(quoteCurrency, new Asset("BNB")), 000306, .25m * deposit / 000340));
    //                                                                                100%

    List<AbsAssetAlloc> absAssetAlloc = new()
    {
      new(new Asset("EUR"), .05m),
      new(new Asset("BTC"), .40m),
      new(new Asset("ETH"), .30m),
      new(new Asset("ADA"), .25m),
      //                    100%
    };

    var quoteDiffs = Trader.GetAllocationQuoteDiffs(absAssetAlloc, curBalance).ToList();

    Assert.AreEqual(5, quoteDiffs.Count);

    Assert.AreEqual(-005, (double)Math.Round(quoteDiffs[0].Value, 1));
    Assert.AreEqual(0040, (double)Math.Round(quoteDiffs[1].Value, 1));
    Assert.AreEqual(0015, (double)Math.Round(quoteDiffs[2].Value, 1));
    Assert.AreEqual(0225, (double)Math.Round(quoteDiffs[3].Value, 1));
    Assert.AreEqual(-275, (double)Math.Round(quoteDiffs[4].Value, 1));
  }
}