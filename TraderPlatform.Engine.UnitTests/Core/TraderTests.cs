using TraderPlatform.Abstracts.Models;
using TraderPlatform.Common.Exchanges;

namespace TraderPlatform.Engine.Extensions.Tests;

[TestClass()]
public class TraderTests
{
  private readonly Asset quoteCurrency = new("EUR");

  private readonly ExchangeMock exchangeService;

  private readonly List<AbsAssetAlloc> absAssetAlloc = new()
  {
    new(new Asset("EUR"), .05m),
    new(new Asset("BTC"), .40m),
    new(new Asset("ETH"), .30m),
    new(new Asset("ADA"), .25m),
    //                    100%
  };

  public TraderTests()
  {
    exchangeService = new(quoteCurrency, 5, .0015m, .0025m);
  }

  [TestMethod()]
  public async Task AllocQuoteDiffsTest()
  {
    Balance curBalance = await exchangeService.GetBalance();

    var allocQuoteDiffs = Common.Functions.Rebalance.GetAllocationQuoteDiffs(absAssetAlloc, curBalance).ToList();

    Assert.AreEqual(5, allocQuoteDiffs.Count);

    Assert.AreEqual(-005, (double)Math.Round(allocQuoteDiffs[0].Value, 1));
    Assert.AreEqual(0040, (double)Math.Round(allocQuoteDiffs[1].Value, 1));
    Assert.AreEqual(0015, (double)Math.Round(allocQuoteDiffs[2].Value, 1));
    Assert.AreEqual(0225, (double)Math.Round(allocQuoteDiffs[3].Value, 1));
    Assert.AreEqual(-275, (double)Math.Round(allocQuoteDiffs[4].Value, 1));
  }

  [TestMethod()]
  public async Task RebalanceTest()
  {
    var rebalanceOrders = (await exchangeService.Rebalance(absAssetAlloc)).ToList();

    Assert.AreEqual(4, rebalanceOrders.Count);

    Assert.AreEqual(1.3875m, Math.Round(rebalanceOrders.Sum(result => result.FeeExpected), 4));

    Assert.IsNull(rebalanceOrders[0].Amount);
    Assert.IsNull(rebalanceOrders[1].Amount);
    Assert.IsNotNull(rebalanceOrders[2].Amount); // expected to sell whole position
    Assert.IsNull(rebalanceOrders[3].Amount);

    Balance curBalance = await exchangeService.GetBalance();

    var allocQuoteDiffs = Common.Functions.Rebalance.GetAllocationQuoteDiffs(absAssetAlloc, curBalance).ToList();

    Assert.AreEqual(5, allocQuoteDiffs.Count);

    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[0].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[1].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[2].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[3].Value));
    Assert.AreEqual(0, (double)Math.Round(allocQuoteDiffs[4].Value));
  }
}