using Microsoft.Extensions.DependencyInjection;
using WebPingzor.Authentication.Login;
using WebPingzor.Authentication.Register;

namespace WebPingzor.Authentication;

public static class AuthenticationModule
{
  public static void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<RegisterService>();
  }
}