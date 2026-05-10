using CaRP.Shared.Dtos;
using Clerk.Net.Client;
using Clerk.Net.Client.Organizations.Item.Invitations;

namespace CaRP.Backend;

public static partial class Endpoints
{
    public class ClerkConnector : BackgroundService
    {
        private readonly ClerkApiClient _clerkApiClient;

        public ClerkConnector(ClerkApiClient clerkApiClient)
        {
            _clerkApiClient = clerkApiClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var invites = await _clerkApiClient.Organizations["CaRP"].Invitations.GetAsync(x =>
            {
                x.QueryParameters.Status = GetStatusQueryParameterType.Pending;
            });
        }
    }

    public static class Secrets {
        public static string SecretKey { get; set; } = string.Empty;
        public static string PublishableKey { get; set; } = string.Empty;
        public static string ConnString { get; set; } = string.Empty;
    }



    public static async void Login(LoginDto data)
    {

    }

}