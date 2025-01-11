using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebPingzor.Web.Client;

Console.WriteLine("Hello from webclient module!");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAntDesign();

await builder.Build().RunAsync();
