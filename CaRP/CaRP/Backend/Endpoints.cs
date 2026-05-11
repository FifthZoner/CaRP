using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using Clerk.Net.Client;

namespace CaRP.Backend;

public static partial class Endpoints
{
    public static void MapEndpoints(RouteGroupBuilder api)
    {
        // testing
        api.MapGet("/test", () => Results.Ok(new { Message = "Backend works!" }));

        api.MapGet("/user/login_details", async (ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Secrets.SecretKey);

            var clerkUser = await client.GetFromJsonAsync<ClerkUserResponse>($"https://api.clerk.com/v1/users/{userId}");

            if (clerkUser == null)
                return Results.InternalServerError("Could not find clerk user");

            var role = user.FindFirst("role_enum_value")?.Value;
            if (!int.TryParse(role, out var roleLevel))
                return Results.InternalServerError("Could not get user role level");

            return Results.Ok(new LoginInfoDto() {
                Username = user.FindFirst("login")?.Value ?? "",
                ImageUrl = clerkUser.ImageUrl ?? null,
                RoleLevel = (RoleEnum)roleLevel
            });
        }).RequireAuthorization();
    }


}