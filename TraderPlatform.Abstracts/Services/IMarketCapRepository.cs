using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.Services;

/// <summary>
/// Retrieves latest market cap data from external service.
/// </summary>
public interface IMarketCapRepository
{
  /// <summary>
  /// Get market cap data for the specified market.
  /// </summary>
  /// <param name="market">Market for which to get market cap data.</param>
  /// <returns>Market cap data.</returns>
  Task<MarketCapDto> GetMarketCap(Market market);

  /// <summary>
  /// Get the latest market cap data of the specified amount of top ranked base currencies for the specified quote currency.
  /// </summary>
  /// <param name="quoteSymbol"></param>
  /// <param name="count"></param>
  /// <returns></returns>
  Task<IEnumerable<MarketCapDto>> ListLatest(string quoteSymbol, int count = 50);
}