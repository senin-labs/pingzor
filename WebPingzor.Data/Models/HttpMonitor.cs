using Microsoft.EntityFrameworkCore;

namespace WebPingzor.Data.Models;

[Index(nameof(UserId), nameof(Name), IsUnique = true)]
public class HttpMonitor
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public User? User { get; set; }

  public required string Name { get; set; }

  public required string Url { get; set; }

  public required int Interval { get; set; }
  public required DateTime NextCheck { get; set; }

  public List<MonitorStatusCheck>? StatusChecks { get; set; }
}
