using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace CaRP.Client.Handlers;

public class ClerkAuthorizationHandler : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;

    public ClerkAuthorizationHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Handler is running..."); // Does this show up?

        var token = await _jsRuntime.InvokeAsync<string>("clerkInterop.getAccessToken");

        Console.WriteLine($"Token retrieved: {(!string.IsNullOrEmpty(token))}");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}