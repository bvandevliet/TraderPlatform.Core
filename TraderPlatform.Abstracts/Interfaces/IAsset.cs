namespace TraderPlatform.Abstracts.Interfaces;

public interface IAsset
{
  string Symbol { get; set; }

  string? Name { get; set; }

  int? Decimals { get; }
}