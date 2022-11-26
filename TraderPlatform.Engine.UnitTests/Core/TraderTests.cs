using TraderPlatform.Abstracts.Models;
using TraderPlatform.Engine.Models;
using TraderPlatform.Engine.UnitTests.Services;

namespace TraderPlatform.Engine.Core.Tests;

[TestClass()]
public class TraderTests
{
  [TestMethod()]
  public async Task GetAllocationQuoteDiffsTest()
  {
    Asset quoteCurrency = new("EUR");

    ExchangeService exchangeService = new(quoteCurrency, 5, .0015m, .0025m);

    Balance curBalance = await exchangeService.GetBalance();

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