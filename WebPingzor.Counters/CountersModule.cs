using Microsoft.Extensions.DependencyInjection;

namespace WebPingzor.Counters;

public static class CountersModule
{
  public static void ConfigureServices(IServiceCollection services)
  {
    services.AddSingleton<CounterServiceSingleton>();
    services.AddScoped<CounterServiceScoped>();
    services.AddTransient<CounterServiceTransient>();
  }
}