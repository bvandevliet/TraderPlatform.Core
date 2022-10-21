using Skender.Stock.Indicators;

namespace TraderPlatform.Common
{
    public static class Math
    {
        private static void Test()
        {
            //IQuote[] quotesOHLC = new Quote[10];

            List<(DateTime Date, double Value)> quotesSingle = new();

            var emaEnum = quotesSingle.GetEma(10);

            double? ema = emaEnum.Last().Ema;
        }
    }
}