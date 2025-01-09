using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;

namespace WebPingzor.Data;

public class PingzorDbProvider(IDbContextFactory<PingzorDbContext> contextFactory)
{
  private IDbContextFactory<PingzorDbContext> ContextFactory { get; } = contextFactory;

  public PingzorDbContext Create()
  {
    return ContextFactory.CreateDbContext();
  }
}