using System.Security.Claims;
using CaRP.Shared.Dtos;
using Clerk.Net.Client;
using Clerk.Net.Client.Organizations.Item.Invitations;
using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using Clerk.Net.Client;

namespace CaRP.Backend;

public static partial class Endpoints
{
    private static Func<ClaimsPrincipal, Task<IResult>> LoginDetails()
    {
        return async (ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Secrets.SecretKey);

            var clerkUser = await client.GetFromJsonAsync<ClerkUserResponse>($"https://api.clerk.com/v1/users/{userId}");

            if (clerkUser == null)
                return Results.InternalServerError("Could not find clerk user");

            var role = GetRole(user);
            if (role == null)
                return Results.NotFound();

            return Results.Ok(new LoginInfoDto() {
                Username = user.FindFirst("login")?.Value ?? "",
                ImageUrl = clerkUser.ImageUrl ?? null,
                RoleLevel = role.Value
            });
        };
    }

    public static RoleEnum? GetRole(ClaimsPrincipal user)
    {
        var role = user.FindFirst("role_enum_value")?.Value;
        if (!int.TryParse(role, out var roleLevel))
            return null;
        return (RoleEnum)roleLevel;
    }

    public static bool IsAtLeast(this ClaimsPrincipal user, RoleEnum atLeastRole)
    {
        var role = GetRole(user);
        if (!role.HasValue)
            return false;
        return (int)role.Value >= (int)atLeastRole;
    }
    public static bool IsNotAtLeast(this ClaimsPrincipal user, RoleEnum atLeastRole)
    {
        return !IsAtLeast(user, atLeastRole);
    }

    public static class Secrets {
        public static string SecretKey { get; set; } = string.Empty;
        public static string PublishableKey { get; set; } = string.Empty;
        public static string ConnString { get; set; } = string.Empty;
    }
}