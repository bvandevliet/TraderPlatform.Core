using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Services;

public interface IExchangeService
{
  string QuoteCurrency { get; }

  decimal MinimumOrderSize { get; }

  decimal MakerFee { get; }

  decimal TakerFee { get; }

  IPortfolioBalance GetBalance();

  // and more ..
}