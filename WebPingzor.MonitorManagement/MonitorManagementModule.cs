using Microsoft.Extensions.DependencyInjection;

namespace WebPingzor.MonitorManagement;

public static class MonitorManagementModule
{
  public static void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<MonitorService>();
  }
}