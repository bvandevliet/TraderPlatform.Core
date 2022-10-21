using Skender.Stock.Indicators;

namespace TraderPlatform.Common;

public static class Indicators
{
  private static double? EmaTest()
  {
    //List<(DateTime Date, double Value)> quotesSingle = new();

    IQuote[] quotesOHLC = new Quote[10];
    IEnumerable<(DateTime Date, double Value)> quotesSingle = quotesOHLC.Use(CandlePart.Close);

    IEnumerable<EmaResult> emaEnum = quotesSingle.GetEma(10);
    return emaEnum.Last().Ema;
  }
}