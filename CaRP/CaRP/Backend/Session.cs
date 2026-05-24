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
using Microsoft.AspNetCore.Mvc;

namespace CaRP.Backend;

public static partial class Endpoints
{
    private static async Task<IResult> LoginDetails(ClaimsPrincipal user, [FromServices] ClerkApiClient clerkClient)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            return Results.InternalServerError("Could not find user id");

        var users = await clerkClient.Users.GetAsync(config =>
        {
            config.QueryParameters.UserId = [userId];
        });

        if (users is not { Count: 1 })
            return Results.NotFound();

        return Results.Ok(ClerkUserToDto(users.First()));
    }

    public static RoleEnum GetRole(string? role)
    {
        if (!int.TryParse(role, out var roleLevel))
            return RoleEnum.Unset;
        return (RoleEnum)roleLevel;
    }

    public static RoleEnum GetRole(ClaimsPrincipal user)
    {
        return GetRole(user.FindFirst("role_enum_value")?.Value);
    }

    public static bool IsAtLeast(this ClaimsPrincipal user, RoleEnum atLeastRole)
    {
        if (user.FindFirst("login") == null)
            return false;
        return (int)GetRole(user) >= (int)atLeastRole;
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

        return permissionCheck(role);
    }

    public static bool HasAny(this ClaimsPrincipal user, params Func<RoleEnum, bool>[] permissionCheck)
    {
        var role = GetRole(user);
        if (role == null)
            return false;

        return permissionCheck.Any(x => x(role));
    }

    public static bool HasAll(this ClaimsPrincipal user, params Func<RoleEnum, bool>[] permissionCheck)
    {
        var role = GetRole(user);
        if (role == null)
            return false;

        return permissionCheck.All(x => x(role));
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