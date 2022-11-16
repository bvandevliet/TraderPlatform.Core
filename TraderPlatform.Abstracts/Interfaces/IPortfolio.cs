namespace TraderPlatform.Abstracts.Interfaces;

/// <summary>
/// Represents a portfolio, containing only absolute asset positions.
/// </summary>
public interface IPortfolio
{
  /// <summary>
  /// Collection of asset positions.
  /// </summary>
  Dictionary<IAsset, IPosition> Positions { get; }
}