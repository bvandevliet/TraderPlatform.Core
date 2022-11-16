global using Microsoft.VisualStudio.TestTools.UnitTesting;

using TraderPlatform.Abstracts.Interfaces;

/// <inheritdoc cref="IAsset"/>
internal class Asset : IAsset
{
  public string Symbol { get; set; }
  public string Name { get; set; } = string.Empty;

  public Asset(string symbol)
  {
    Symbol = symbol;
  }
}

/// <inheritdoc cref="IMarket"/>
internal class Market : IMarket
{
  public IAsset QuoteCurrency { get; set; }
  public IAsset BaseCurrency { get; set; }

  public Market(Asset quoteCurrency, Asset baseCurrency)
  {
    QuoteCurrency = quoteCurrency;
    BaseCurrency = baseCurrency;
  }
}