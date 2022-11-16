namespace TraderPlatform.Abstracts.Enums;

/// <summary>
/// Only for limit orders. Determines how long orders remain active.
/// </summary>
public enum TimeInForce
{
  /// <summary>
  /// Good-Til-Canceled
  /// </summary>
  GTC,
  /// <summary>
  /// Immediate-Or-Cancel
  /// </summary>
  IOC,
  /// <summary>
  /// Fill-Or-Kill
  /// </summary>
  FOK,
}