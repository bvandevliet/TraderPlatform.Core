using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;

namespace TraderPlatform.Daemon.Services;

public class MarketCapRepository : IMarketCapRepository
{
  public Task<MarketCapDto> GetMarketCap(Market market)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<MarketCapDto>> ListLatest(string quoteSymbol, int count = 50)
  {
    throw new NotImplementedException();
  }
}