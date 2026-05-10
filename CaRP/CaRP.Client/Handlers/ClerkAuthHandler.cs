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
        var token = await _jsRuntime.InvokeAsync<string>("clerkInterop.getAccessToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}