global using Microsoft.VisualStudio.TestTools.UnitTesting;

using TraderPlatform.Abstracts.Interfaces;

/// <inheritdoc cref="IMarket"/>
internal class Market : IMarket
{
  public IAsset QuoteCurrency => throw new NotImplementedException();
  public IAsset BaseCurrency => throw new NotImplementedException();
}