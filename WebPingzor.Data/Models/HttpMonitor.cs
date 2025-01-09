namespace WebPingzor.Data.Models;

public class HttpMonitor
{
  public int Id { get; set; }

  public required string Name { get; set; }

  public required string Url { get; set; }

  public required int Interval { get; set; }
}
