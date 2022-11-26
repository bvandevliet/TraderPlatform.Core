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

    var quoteDiffs = Trader.GetAllocationQuoteDiffs(absAssetAlloc, curBalance).ToList();

    Assert.AreEqual(5, quoteDiffs.Count);

    Assert.AreEqual(-005, (double)Math.Round(quoteDiffs[0].Value, 1));
    Assert.AreEqual(0040, (double)Math.Round(quoteDiffs[1].Value, 1));
    Assert.AreEqual(0015, (double)Math.Round(quoteDiffs[2].Value, 1));
    Assert.AreEqual(0225, (double)Math.Round(quoteDiffs[3].Value, 1));
    Assert.AreEqual(-275, (double)Math.Round(quoteDiffs[4].Value, 1));

    var resultsSimulation = exchangeService.SimulateRebalance(absAssetAlloc, curBalance).ToList();

    // Sell pieces of oversized allocations first,
    // so we have sufficient quote currency available to buy with.
    IOrder[] sellResults = await exchangeService.SellOveragesAndVerify(absAssetAlloc, curBalance);

    // Then update balance the dirty way.
    curBalance.GetAllocation(quoteCurrency)!.AmountQuote += (40 + 15 + 225);
    curBalance.GetAllocation(new Asset("BTC"))!.AmountQuote -= 40;
    curBalance.GetAllocation(new Asset("ETH"))!.AmountQuote -= 15;
    curBalance.GetAllocation(new Asset("BNB"))!.AmountQuote -= 225;

    // Then buy to increase undersized allocations.
    IOrder[] buyResults = await exchangeService.BuyUnderages(absAssetAlloc, curBalance);

    // Results ..
    var resultsRebalance = sellResults.Concat(buyResults).ToList();

    Assert.AreEqual(1.3875m, Math.Round(resultsRebalance.Sum(result => result.FeeExpected), 4));
    Assert.AreEqual(1.3875m, Math.Round(resultsSimulation.Sum(result => result.FeeExpected), 4));

    Assert.AreEqual(4, resultsRebalance.Count);
    Assert.AreEqual(4, resultsSimulation.Count);

    Assert.IsNull(resultsRebalance[0].Amount);
    Assert.IsNull(resultsRebalance[1].Amount);
    Assert.IsNotNull(resultsRebalance[2].Amount); // expected to sell whole position
    Assert.IsNull(resultsRebalance[3].Amount);

    Assert.AreEqual(resultsRebalance[0].Amount, resultsSimulation[0].Amount);
    Assert.AreEqual(resultsRebalance[1].Amount, resultsSimulation[1].Amount);
    Assert.AreEqual(resultsRebalance[2].Amount, resultsSimulation[2].Amount);
    Assert.AreEqual(resultsRebalance[3].Amount, resultsSimulation[3].Amount);

    Assert.IsNotNull(resultsRebalance[0].AmountQuote);
    Assert.IsNotNull(resultsRebalance[1].AmountQuote);
    Assert.IsNull(resultsRebalance[2].AmountQuote); // expected to sell whole position
    Assert.IsNotNull(resultsRebalance[3].AmountQuote);

    Assert.AreEqual(resultsRebalance[0].AmountQuote, resultsSimulation[0].AmountQuote);
    Assert.AreEqual(resultsRebalance[1].AmountQuote, resultsSimulation[1].AmountQuote);
    Assert.AreEqual(resultsRebalance[2].AmountQuote, resultsSimulation[2].AmountQuote);
    Assert.AreEqual(resultsRebalance[3].AmountQuote, resultsSimulation[3].AmountQuote);
  }
}