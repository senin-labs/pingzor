using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using WebPingzor.Core;
using WebPingzor.Web.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();
builder.Services.AddScoped<IToasterService, ToasterService>();
builder.Services.AddSingleton<CircuitHandler, ConnectionTracker>();
builder.Services.AddSingleton<IWebSocketTracker>(sp => sp.GetRequiredService<CircuitHandler>() as IWebSocketTracker);

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAntDesign();

WebPingzor.Authentication.AuthenticationModule.ConfigureServices(builder.Services);
WebPingzor.Data.DataModule.ConfigureServerServices(builder.Services);
WebPingzor.MonitorManagement.MonitorManagementModule.ConfigureServices(builder.Services);
WebPingzor.MonitorBackgroundWorkers.MonitorBackgroundWorkersModule.ConfigureServices(builder.Services);
WebPingzor.Counters.CountersModule.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope())
{
    await WebPingzor.Data.DataModule.Run(scope);
}


app.UseHttpsRedirection();


app.MapStaticAssets();
app.UseRouting();
app.UseAntiforgery();

app.UseAuthorization();
app.MapControllers();


app.MapRazorComponents<WebPingzor.Web.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(WebPingzor.MonitorManagement.MonitorManagementModule).Assembly,
        typeof(WebPingzor.Authentication.AuthenticationModule).Assembly,
        typeof(WebPingzor.Counters.CountersModule).Assembly,
        typeof(WebPingzor.UserManagement.UserManagementModule).Assembly,
        typeof(WebPingzor.Data.DataModule).Assembly
    );

app.Run();
