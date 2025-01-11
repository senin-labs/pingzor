namespace WebPingzor.Counters;

internal class CounterServiceSingleton
{
  public int Counter { get; private set; }

  public void Increment()
  {
    Counter++;
    Console.WriteLine("CounterServiceSingleton: Incremented to {0}", Counter);
    OnChange?.Invoke(Counter);
  }

  public event Func<int, Task>? OnChange;
}