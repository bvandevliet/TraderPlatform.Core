using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Engine.Models;
using TraderPlatform.Engine.UnitTests.Services;

namespace TraderPlatform.Engine.Core.Tests;

[TestClass()]
public class TraderTests
{
  [TestMethod()]
  public async Task RebalanceTest()
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

    var allocQuoteDiffs = Trader.GetAllocationQuoteDiffs(absAssetAlloc, curBalance).ToList();

    Assert.AreEqual(5, allocQuoteDiffs.Count);

    Assert.AreEqual(-005, (double)Math.Round(allocQuoteDiffs[0].Value, 1));
    Assert.AreEqual(0040, (double)Math.Round(allocQuoteDiffs[1].Value, 1));
    Assert.AreEqual(0015, (double)Math.Round(allocQuoteDiffs[2].Value, 1));
    Assert.AreEqual(0225, (double)Math.Round(allocQuoteDiffs[3].Value, 1));
    Assert.AreEqual(-275, (double)Math.Round(allocQuoteDiffs[4].Value, 1));

    // Results ..
    var resultsRebalance = (await exchangeService.Rebalance(absAssetAlloc)).ToList();

    Assert.AreEqual(1.3875m, Math.Round(resultsRebalance.Sum(result => result.FeeExpected), 4));

    Assert.AreEqual(4, resultsRebalance.Count);

    Assert.IsNull(resultsRebalance[0].Amount);
    Assert.IsNull(resultsRebalance[1].Amount);
    Assert.IsNotNull(resultsRebalance[2].Amount); // expected to sell whole position
    Assert.IsNull(resultsRebalance[3].Amount);

    curBalance = await exchangeService.GetBalance();

    allocQuoteDiffs = Trader.GetAllocationQuoteDiffs(absAssetAlloc, curBalance).ToList();

    Assert.AreEqual(5, allocQuoteDiffs.Count);

    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[0].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[1].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[2].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[3].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[4].Value));
  }
}