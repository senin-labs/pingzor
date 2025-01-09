using Microsoft.Extensions.DependencyInjection;

namespace WebPingzor.Monitoring;

public static class MonitoringModule
{
  public static void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<MonitorService>();
  }
}