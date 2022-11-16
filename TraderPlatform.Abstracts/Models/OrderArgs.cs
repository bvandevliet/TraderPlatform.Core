using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IOrderArgs"/>
public class OrderArgs : IOrderArgs
{
  /// <inheritdoc cref="IOrderArgs.Price"/>
  public decimal? Price { get; set; } = null;

  /// <inheritdoc cref="IOrderArgs.Amount"/>
  public decimal? Amount { get; set; } = null;

  /// <inheritdoc cref="IOrderArgs.AmountQuote"/>
  public decimal? AmountQuote { get; set; } = null;

  /// <inheritdoc cref="IOrderArgs.TimeInForce"/>
  public Enums.TimeInForce TimeInForce { get; set; } = Enums.TimeInForce.GTC;
}