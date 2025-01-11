using Microsoft.EntityFrameworkCore;
using WebPingzor.Data.Models;

namespace WebPingzor.Data;

public sealed class PingzorDbContext(DbContextOptions<PingzorDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }

  public DbSet<HttpMonitor> HttpMonitors { get; set; }
}
