using CaRP.Client.Handlers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CaRP.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped<ClerkAuthorizationHandler>();

// 2. Register an HttpClient that uses this handler
        builder.Services.AddHttpClient("api", client =>
            {
                // This automatically matches http://localhost:5249 or https://your-app.azurewebsites.net
                client.BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "api/");
            })
            .AddHttpMessageHandler<ClerkAuthorizationHandler>();


// 3. (Optional) Set the default HttpClient to use this handler
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));


        await builder.Build().RunAsync();
    }
}