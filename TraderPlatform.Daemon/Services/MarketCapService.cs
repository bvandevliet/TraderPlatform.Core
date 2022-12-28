using Skender.Stock.Indicators;
using TraderPlatform.Abstracts.Models;
using TraderPlatform.Abstracts.Services;
using TraderPlatform.Common.Extensions;

namespace TraderPlatform.Daemon.Services;

public class MarketCapService : IMarketCapService
{
  private readonly Dictionary<string, IEnumerable<MarketCapDto>> listLatestCache = new();
  private readonly Dictionary<string, IEnumerable<IEnumerable<MarketCapDto>>> listHistoricalCache = new();
  private readonly Dictionary<string, IEnumerable<MarketCapDto>> listLatestSmoothedCache = new();

  public Task<MarketCapDto> GetMarketCap(Market market)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<MarketCapDto>> ListLatest(string quoteSymbol, int count = 50)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<IEnumerable<MarketCapDto>>> ListHistorical(string quoteSymbol, int count = 50, int days = 21)
  {
    throw new NotImplementedException();
  }

  public async Task<IEnumerable<MarketCapDto>> ListLatest(string quoteSymbol, int count = 50, int smoothing = 20)
  {
    string cacheHash = $"{quoteSymbol}-{count}-{smoothing}";

    if (!listLatestSmoothedCache.ContainsKey(cacheHash))
    {
      IEnumerable<IEnumerable<MarketCapDto>> marketCapsHistorical = await ListHistorical(quoteSymbol, count, smoothing + 1);

      listLatestSmoothedCache.Add(cacheHash,
        marketCapsHistorical.Select(marketCaps =>
        {
          double? marketCapEma = marketCaps.GetEma(smoothing).Last().Ema;

          MarketCapDto marketCap = marketCaps.Last();

          marketCap.MarketCap = marketCapEma ?? 0;

          return marketCap;
        }));
    }

    return listLatestSmoothedCache[cacheHash];
  }
}