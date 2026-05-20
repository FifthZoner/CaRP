using System.Security.Claims;
using CaRP.Shared.Dtos;
using Clerk.Net.Client;
using Clerk.Net.Client.Organizations.Item.Invitations;
using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using carp.Shared.Permissions;
using Clerk.Net.Client;

namespace CaRP.Backend;

public static partial class Endpoints
{
    private static async Task<IResult> LoginDetails(ClaimsPrincipal user)
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
            RoleLevel = role.Value,
            FirstName = clerkUser.FirstName,
            LastName = clerkUser.LastName
        });
    }

    public static RoleEnum? GetRole(string? role)
    {
        if (!int.TryParse(role, out var roleLevel))
        // by default let's put in that the user is a driver, reduces the need for messing around in clerk dashboard
            return RoleEnum.Driver;
        return (RoleEnum)roleLevel;
    }

    public static RoleEnum? GetRole(ClaimsPrincipal user)
    {
        return GetRole(user.FindFirst("role_enum_value")?.Value);
    }

    public static bool IsAtLeast(this ClaimsPrincipal user, RoleEnum atLeastRole)
    {
        var role = GetRole(user);
        if (!role.HasValue || user.FindFirst("login") == null)
            return false;
        return (int)role.Value >= (int)atLeastRole;
    }
    public static bool IsNotAtLeast(this ClaimsPrincipal user, RoleEnum atLeastRole)
    {
        return !IsAtLeast(user, atLeastRole);
    }

    public static string Login(this ClaimsPrincipal user) => user.FindFirst("login")?.Value ?? "";


    public static bool Has(this ClaimsPrincipal user, Func<RoleEnum, bool> permissionCheck)
    {
        var role = GetRole(user);
        if (role == null)
            return false;

        return permissionCheck(role.Value);
    }

    public static bool HasAny(this ClaimsPrincipal user, params Func<RoleEnum, bool>[] permissionCheck)
    {
        var role = GetRole(user);
        if (role == null)
            return false;

        return permissionCheck.Any(x => x(role.Value));
    }

    public static bool HasAll(this ClaimsPrincipal user, params Func<RoleEnum, bool>[] permissionCheck)
    {
        var role = GetRole(user);
        if (role == null)
            return false;

        return permissionCheck.All(x => x(role.Value));
    }

    public static bool CanWrite(this ClaimsPrincipal user, in Perms permissions, in string dtoLogin)
    {
        if (string.IsNullOrWhiteSpace(dtoLogin))
            return false;

        if (user.HasAll(Perms.Services.CanFullAll))
            return true;

        if (!user.HasAll(Perms.Services.CanFullOwn))
            return false;

        return user.Login() == dtoLogin;
    }

    public static bool CanRead(this ClaimsPrincipal user, in Perms permissions, in string dtoLogin)
    {
        if (string.IsNullOrWhiteSpace(dtoLogin))
            return false;

        if (user.HasAll(Perms.Services.CanReadAll))
            return true;

        if (!user.HasAll(Perms.Services.CanReadOwn))
            return false;

        return user.Login() == dtoLogin;
    }

    public static class Secrets {
        public static string SecretKey { get; set; } = string.Empty;
        public static string PublishableKey { get; set; } = string.Empty;
        public static string ConnString { get; set; } = string.Empty;
    }
}