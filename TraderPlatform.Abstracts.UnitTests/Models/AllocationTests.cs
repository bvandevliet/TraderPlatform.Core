using TraderPlatform.Abstracts.Interfaces;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.UnitTests.Models;

[TestClass]
public class AllocationTests
{
  private readonly Asset quoteCurrency = new("EUR");
  private readonly Asset baseCurrency = new("BTC");

  /// <inheritdoc/>
  private class AllocationWrapper : Allocation
  {
    private bool priceUpdate = false;
    private bool amountUpdate = false;
    private bool amountAvailableUpdate = false;

    /// <inheritdoc/>
    public AllocationWrapper(
      IMarket market,
      decimal price,
      decimal amount,
      decimal? amountAvailable = null)
      : base(market, price, amount, amountAvailable)
    {
      PriceUpdate += (sender, e) => priceUpdate = true;
      AmountUpdate += (sender, e) => amountUpdate = true;
      AmountAvailableUpdate += (sender, e) => amountAvailableUpdate = true;
    }

    internal bool PriceUpdateEventTriggered() => priceUpdate || (priceUpdate = false);
    internal bool AmountUpdateEventTriggered() => amountUpdate || (amountUpdate = false);
    internal bool AmountAvailableUpdateEventTriggered() => amountAvailableUpdate || (amountAvailableUpdate = false);
  }

  [TestMethod]
  public void InitTooGreatAmountAvailable()
  {
    decimal price = 15;
    decimal amount = 25;
    decimal amountAvailable = amount + 5; // should be 25

    // Create instance, but pass too great value for AmountAvailable.
    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount, amountAvailable);

    // Reset to expected value.
    amountAvailable = amount;

    // AmountAvailable should be corrected to not be greater than but equal to Amount.
    Assert.AreEqual(amountAvailable, alloc.AmountAvailable);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
    Assert.AreEqual(amountAvailable * price, alloc.AmountQuoteAvailable);
  }

  [TestMethod]
  public void InitOmitAmountAvailable()
  {
    decimal price = 15;
    decimal amount = 25;
    decimal amountAvailable = amount; // 25

    // Create instance, but omit AmountAvailable.
    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount/*, amountAvailable*/);

    // AmountAvailable should be initiated to be equal to Amount.
    Assert.AreEqual(amountAvailable, alloc.AmountAvailable);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
    Assert.AreEqual(amountAvailable * price, alloc.AmountQuoteAvailable);
  }

  [TestMethod]
  public void UpdatePrice()
  {
    decimal price = 15;
    decimal amount = 25;
    decimal amountAvailable = amount; // 25

    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount);

    price = alloc.Price = 10; // was 15

    // Test if events are raised (or not) as expected.
    Assert.IsTrue(alloc.PriceUpdateEventTriggered());
    Assert.IsFalse(alloc.AmountUpdateEventTriggered());
    Assert.IsFalse(alloc.AmountAvailableUpdateEventTriggered());

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
    Assert.AreEqual(amountAvailable * price, alloc.AmountQuoteAvailable);
  }

  [TestMethod]
  public void IncreaseAmount()
  {
    decimal price = 15;
    decimal amount = 25;
    decimal amountAvailable = amount; // 25

    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount, amountAvailable);

    amount = alloc.Amount = 30; // was 25

    // Test if event is not raised.
    Assert.IsFalse(alloc.PriceUpdateEventTriggered());

    // Test if event is raised.
    Assert.IsTrue(alloc.AmountUpdateEventTriggered());

    // Test if event is not raised, because Amount was set above AmountAvailable.
    Assert.IsFalse(alloc.AmountAvailableUpdateEventTriggered());

    // Test if increased accordingly.
    Assert.AreEqual(amount, alloc.Amount);

    // Test if not changed, because Amount was set above AmountAvailable.
    Assert.AreEqual(amountAvailable, alloc.AmountAvailable);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
    Assert.AreEqual(amountAvailable * price, alloc.AmountQuoteAvailable);
  }

  [TestMethod]
  public void DecreaseAmount()
  {
    decimal price = 15;
    decimal amount = 25;
    decimal amountAvailable = amount; // 25

    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount, amountAvailable);

    amount = amountAvailable = alloc.Amount = 20; // was 25

    // Test if event is not raised.
    Assert.IsFalse(alloc.PriceUpdateEventTriggered());

    // Test if event is raised.
    Assert.IsTrue(alloc.AmountUpdateEventTriggered());

    // Test if event is raised, because Amount was set below AmountAvailable.
    Assert.IsTrue(alloc.AmountAvailableUpdateEventTriggered());

    // Test if decreased accordingly.
    Assert.AreEqual(amount, alloc.Amount);
    Assert.AreEqual(amountAvailable, alloc.AmountAvailable);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
    Assert.AreEqual(amountAvailable * price, alloc.AmountQuoteAvailable);
  }

  [TestMethod]
  public void DecreaseAmountAvailable()
  {
    decimal price = 15;
    decimal amount = 25;
    decimal amountAvailable = amount; // 25

    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount, amountAvailable);

    amountAvailable = alloc.AmountAvailable = 20; // was 25

    // Test if event is not raised.
    Assert.IsFalse(alloc.PriceUpdateEventTriggered());

    // Test if event is not raised, because AmountAvailable was set below Amount.
    Assert.IsFalse(alloc.AmountUpdateEventTriggered());

    // Test if event is raised.
    Assert.IsTrue(alloc.AmountAvailableUpdateEventTriggered());

    // Test if not changed, because AmountAvailable was set below Amount.
    Assert.AreEqual(amount, alloc.Amount);

    // Test if decreased accordingly.
    Assert.AreEqual(amountAvailable, alloc.AmountAvailable);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
    Assert.AreEqual(amountAvailable * price, alloc.AmountQuoteAvailable);
  }

  [TestMethod]
  public void IncreaseAmountAvailable()
  {
    decimal price = 15;
    decimal amount = 25;
    decimal amountAvailable = amount; // 25

    // Create instance.
    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount, amountAvailable);

    amountAvailable = amount = alloc.AmountAvailable = 30; // was 25

    // Test if event is not raised.
    Assert.IsFalse(alloc.PriceUpdateEventTriggered());

    // Test if event is raised, because AmountAvailable was set above Amount.
    Assert.IsTrue(alloc.AmountUpdateEventTriggered());

    // Test if event is raised.
    Assert.IsTrue(alloc.AmountAvailableUpdateEventTriggered());

    // Test if increased accordingly.
    Assert.AreEqual(amount, alloc.Amount);
    Assert.AreEqual(amountAvailable, alloc.AmountAvailable);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
    Assert.AreEqual(amountAvailable * price, alloc.AmountQuoteAvailable);
  }
}