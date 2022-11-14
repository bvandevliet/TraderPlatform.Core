namespace TraderPlatform.Abstracts.Interfaces;

public interface IPosition
{
  decimal Amount { get; }

  decimal AmountAvailable { get; }
}