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

    /// <inheritdoc/>
    public AllocationWrapper(
      Market market,
      decimal price,
      decimal amount)
      : base(market, price, amount)
    {
      OnPriceUpdate += (sender, e) => priceUpdate = true;
      OnAmountUpdate += (sender, e) => amountUpdate = true;
    }

    internal bool PriceUpdateEventTriggered() => priceUpdate && !(priceUpdate = false);
    internal bool AmountUpdateEventTriggered() => amountUpdate && !(amountUpdate = false);
  }

  [TestMethod]
  public void Initialize()
  {
    decimal price = 15;
    decimal amount = 25;

    // Create instance.
    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
  }

  [TestMethod]
  public void UpdatePrice()
  {
    decimal price = 15;
    decimal amount = 25;

    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount);

    price = alloc.Price = 10; // was 15

    // Test if events are raised (or not) as expected.
    Assert.IsTrue(alloc.PriceUpdateEventTriggered());
    Assert.IsFalse(alloc.AmountUpdateEventTriggered());

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
  }

  [TestMethod]
  public void IncreaseAmount()
  {
    decimal price = 15;
    decimal amount = 25;

    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount);

    amount = alloc.Amount = 30; // was 25

    // Test if event is not raised.
    Assert.IsFalse(alloc.PriceUpdateEventTriggered());

    // Test if event is raised.
    Assert.IsTrue(alloc.AmountUpdateEventTriggered());

    // Test if increased accordingly.
    Assert.AreEqual(amount, alloc.Amount);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
  }

  [TestMethod]
  public void DecreaseAmount()
  {
    decimal price = 15;
    decimal amount = 25;

    var alloc = new AllocationWrapper(new Market(quoteCurrency, baseCurrency), price, amount);

    amount = alloc.Amount = 20; // was 25

    // Test if event is not raised.
    Assert.IsFalse(alloc.PriceUpdateEventTriggered());

    // Test if event is raised.
    Assert.IsTrue(alloc.AmountUpdateEventTriggered());

    // Test if decreased accordingly.
    Assert.AreEqual(amount, alloc.Amount);

    // Test if quote amounts are correct.
    Assert.AreEqual(amount * price, alloc.AmountQuote);
  }
}