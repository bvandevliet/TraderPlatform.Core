namespace TraderPlatform.Abstracts.EventArgs;

public class NumberUpdateEventArgs : System.EventArgs
{
  public NumberUpdateEventArgs(decimal? oldValue, decimal? newValue)
  {
    OldValue = oldValue;
    NewValue = newValue;
  }

  public decimal? OldValue { get; }
  public decimal? NewValue { get; }
}