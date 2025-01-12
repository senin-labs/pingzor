using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace WebPingzor.Data
{
  public static class DataModule
  {
    public static void ConfigureServerServices(IServiceCollection services)
    {
      services.AddDbContextFactory<PingzorDbContext>(opt =>
opt.UseSqlite($"Data Source=__data/{nameof(PingzorDbContext)}9.db"));

      services.AddScoped<PingzorDbProvider>();
      services.AddScoped<UserService>();
    }

    public async static Task Run(AsyncServiceScope scope)
    {
      await PingzorDbSeed.SeedAll(scope);
    }
  }
}

