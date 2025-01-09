using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WebPingzor.Data.Models;

namespace WebPingzor.Data;

public static class PingzorDbSeed
{
  public static async Task SeedAll(AsyncServiceScope scope)
  {
    var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<PingzorDbContext>>();

    await using var context = new PingzorDbContext(options);
    await context.Database.EnsureCreatedAsync();
    await SeedUsers(context);
    await SeedHttpMonitors(context);
  }

  public static async Task SeedUsers(PingzorDbContext context)
  {
    if (context.Users.Any())
    {
      return;
    }

    var users = new User[]
    {
      new () { Name = "John Travolta", Email="john.travolta@email.com"},
      new () { Name = "Tom Cruise", Email="tom.cruise@email.com"},
      new () { Name = "Brad Pitt", Email="brad.pitt@email.com"},
      new () { Name = "Angelina Jolie", Email="angelina.jolie@email.com"},
    };

    foreach (var user in users)
    {
      context.Users.Add(user);
    }

    await context.SaveChangesAsync();
  }

  public static async Task SeedHttpMonitors(PingzorDbContext context)
  {
    if (context.HttpMonitors.Any())
    {
      return;
    }

    var httpMonitors = new HttpMonitor[]
    {
      new () { Name = "Google", Url = "https://www.google.com", Interval = 60 },
      new () { Name = "Bing", Url = "https://www.bing.com", Interval = 60 },
      new () { Name = "Yahoo", Url = "https://www.yahoo.com", Interval = 60 },
    };

    foreach (var httpMonitor in httpMonitors)
    {
      context.HttpMonitors.Add(httpMonitor);
    }

    await context.SaveChangesAsync();
  }
}
