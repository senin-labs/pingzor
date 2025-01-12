using Microsoft.Extensions.DependencyInjection;

namespace WebPingzor.MonitorBackgroundWorkers;

public static class MonitorBackgroundWorkersModule
{
  public static void ConfigureServices(IServiceCollection services)
  {
    services.AddHostedService<HttpMonitorWorker>();
  }
}