namespace WebPingzor.Data.Models;

public class MonitorStatusCheck
{
  public int Id { get; set; }
  public int MonitorId { get; set; }
  public HttpMonitor? Monitor { get; set; }

  public DateTime CheckedAt { get; set; }
  public bool IsOnline { get; set; }
  public int StatusCode { get; set; }
  public int Latency { get; set; }
  public string? StatusMessage { get; set; }
}