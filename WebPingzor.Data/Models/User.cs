using Microsoft.EntityFrameworkCore;

namespace WebPingzor.Data.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
  public int Id { get; set; }

  public required string Name { get; set; }

  public required string Email { get; set; }

  public required string HashedPassword { get; set; }
  public required string PasswordSalt { get; set; }

  public List<HttpMonitor>? HttpMonitors { get; set; }
}
