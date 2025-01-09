using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebPingzor.Web.Client;

Console.WriteLine("Hello from webclient module!");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

await builder.Build().RunAsync();
