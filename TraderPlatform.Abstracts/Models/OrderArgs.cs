using TraderPlatform.Abstracts.Interfaces;

namespace TraderPlatform.Abstracts.Models;

/// <inheritdoc cref="IOrderArgs"/>
public class OrderArgs : IOrderArgs
{
  /// <inheritdoc/>
  public decimal? Price { get; set; } = null;

  /// <inheritdoc/>
  public decimal? Amount { get; set; } = null;

  /// <inheritdoc/>
  public decimal AmountQuote { get; set; }

  /// <inheritdoc/>
  public Enums.TimeInForce TimeInForce { get; set; } = Enums.TimeInForce.GTC;

  /// <summary>
  /// <inheritdoc cref="IOrderArgs"/>
  /// </summary>
  /// <param name="amountQuote"></param>
  public OrderArgs(decimal amountQuote)
  {
    AmountQuote = amountQuote;
  }
}