using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Abstracts.Services;

/// <summary>
/// Retrieves and processes historical market cap data.
/// </summary>
public interface IMarketCapService : IMarketCapRepository
{
  /// <summary>
  /// Get historical market cap data of the specified amount of top ranked base currencies for the specified quote currency for each day of given amount of days ago.
  /// </summary>
  /// <param name="quoteSymbol"></param>
  /// <param name="count"></param>
  /// <param name="days"></param>
  /// <returns></returns>
  Task<IEnumerable<IEnumerable<MarketCapDto>>> ListHistorical(string quoteSymbol, int count = 50, int days = 21);

  /// <summary>
  /// Get the latest market cap data of the specified amount of top ranked base currencies for the specified quote currency,
  /// smoothing out volatility using an Exponential Moving Average of given amount of smoothing days.
  /// </summary>
  /// <param name="quoteSymbol"></param>
  /// <param name="count"></param>
  /// <param name="smoothing"></param>
  /// <returns></returns>
  Task<IEnumerable<MarketCapDto>> ListLatest(string quoteSymbol, int count = 50, int smoothing = 20);
}