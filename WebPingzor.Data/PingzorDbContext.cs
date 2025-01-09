using Microsoft.EntityFrameworkCore;
using WebPingzor.Data.Models;

namespace WebPingzor.Data;

public sealed class PingzorDbContext : DbContext
{
  public PingzorDbContext(DbContextOptions<PingzorDbContext> options) : base(options)
  {
  }

  public DbSet<User> Users { get; set; }

  public DbSet<HttpMonitor> HttpMonitors { get; set; }
}
