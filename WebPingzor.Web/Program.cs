var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();
WebPingzor.Data.DataModule.ConfigureServerServices(builder.Services);

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
app.MapControllers();

// app.MapGet("/", () => "Hello World!");


app.MapRazorComponents<WebPingzor.Web.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(WebPingzor.Monitoring.MonitoringModule).Assembly,
        typeof(WebPingzor.Authentication.AuthenticationModule).Assembly,
        typeof(WebPingzor.Transactions.TransactionsModule).Assembly,
        typeof(WebPingzor.UserManagement.UserManagementModule).Assembly,
        typeof(WebPingzor.Data.DataModule).Assembly
    );

app.Run();
