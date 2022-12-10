namespace TraderPlatform.Abstracts.EventArgs;

public class NumberUpdateEventArgs : System.EventArgs
{
  public decimal? OldValue { get; }
  public decimal? NewValue { get; }

  public NumberUpdateEventArgs(decimal? oldValue, decimal? newValue)
  {
    OldValue = oldValue;
    NewValue = newValue;
  }
}