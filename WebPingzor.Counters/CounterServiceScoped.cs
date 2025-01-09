namespace WebPingzor.Counters;

internal class CounterServiceScoped
{
  public int Counter { get; private set; }

  public void Increment()
  {
    Counter++;
    Console.WriteLine("CounterServiceScoped: Incremented to {0}", Counter);
  }
}