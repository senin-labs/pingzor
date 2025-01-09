namespace WebPingzor.Counters;

internal class CounterServiceTransient
{
  public int Counter { get; private set; }

  public void Increment()
  {
    Counter++;
    Console.WriteLine("CounterServiceTransient: Incremented to {0}", Counter);
  }
}