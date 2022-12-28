using Skender.Stock.Indicators;
using TraderPlatform.Abstracts.Models;

namespace TraderPlatform.Common.Extensions;

public static partial class IndicatorExtensions
{
  public static IEnumerable<EmaResult> GetEma(this IEnumerable<MarketCapDto> marketCaps, int lookbackPeriods)
  {
    return ((IEnumerable<(DateTime Date, double Value)>)marketCaps
      .Select(marketCap => (marketCap.Updated, marketCap.MarketCap)))
      .GetEma(lookbackPeriods);
  }
}