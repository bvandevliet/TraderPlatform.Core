namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Represents a position in an object.
/// </summary>
public interface IPosition
{
  /// <summary>
  /// Total amount.
  /// </summary>
  decimal Amount { get; }
}