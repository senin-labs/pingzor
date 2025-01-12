using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WebPingzor.Data.Models;
using WebPingzor.Core;

namespace WebPingzor.Data;

public static class PingzorDbSeed
{
  public static async Task SeedAll(AsyncServiceScope scope)
  {
    var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<PingzorDbContext>>();
    var hashingService = scope.ServiceProvider.GetRequiredService<IPasswordHashingService>();

    await using var context = new PingzorDbContext(options);
    await context.Database.EnsureCreatedAsync();
    await SeedUsers(context, hashingService);
  }

  public static async Task SeedUsers(PingzorDbContext context, IPasswordHashingService passwordHashing)
  {
    if (context.Users.Any())
    {
      return;
    }

    var salt = passwordHashing.GenerateSalt();
    var hashedPassword = passwordHashing.HashPassword("pass", salt);

    var users = new User[]
    {
      new () { Name = "Razvan Dragomir", Email="razvan@email.com", HashedPassword = hashedPassword, PasswordSalt = salt},
      new () { Name = "Tom Cruise", Email="tom.cruise@email.com", HashedPassword = hashedPassword, PasswordSalt = salt},
      new () { Name = "Brad Pitt", Email="brad.pitt@email.com", HashedPassword = hashedPassword, PasswordSalt = salt},
      new () { Name = "Angelina Jolie", Email="angelina.jolie@email.com", HashedPassword = hashedPassword, PasswordSalt = salt},
    };

    foreach (var user in users)
    {
      context.Users.Add(user);
    }

    await context.SaveChangesAsync();

    foreach (var user in users)
    {
      await SeedHttpMonitors(context, user.Id);
    }
  }

  public static async Task SeedHttpMonitors(PingzorDbContext context, int userId)
  {
    var httpMonitors = new HttpMonitor[]
    {
      new () { UserId = userId, Name = "Google", Url = "https://www.google.com", Interval = 60, NextCheck = DateTime.Now, IsOnline = true, IsPaused = false },
      new () { UserId = userId, Name = "Bing", Url = "https://www.bing.com", Interval = 60, NextCheck = DateTime.Now, IsOnline = true, IsPaused = false },
      new () { UserId = userId, Name = "Yahoo", Url = "https://www.yahoo.com", Interval = 120, NextCheck = DateTime.Now, IsOnline = true, IsPaused = false },
      new () { UserId = userId, Name = "Some invalid url", Url = "https://www.inva23dfe2.com", Interval = 120, NextCheck = DateTime.Now, IsOnline = true, IsPaused = false },
    };

    foreach (var httpMonitor in httpMonitors)
    {
      context.HttpMonitors.Add(httpMonitor);
    }

    await context.SaveChangesAsync();
  }
}
