global using Microsoft.VisualStudio.TestTools.UnitTesting;

using TraderPlatform.Abstracts.Interfaces;

/// <inheritdoc cref="IAsset"/>
internal class Asset : IAsset
{
  public string Symbol { get; set; }
  public string? Name => throw new NotImplementedException();

  public Asset(string symbol)
  {
    Symbol = symbol;
  }
}

/// <inheritdoc cref="IMarket"/>
internal class Market : IMarket
{
  public IAsset QuoteCurrency { get; set; }
  public IAsset BaseCurrency => throw new NotImplementedException();

  public Market(Asset quoteCurrency)
  {
    QuoteCurrency = quoteCurrency;
  }
}