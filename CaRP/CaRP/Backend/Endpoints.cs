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

        api.MapGet("/user/login_details", LoginDetails()).RequireAuthorization();

        api.MapGet("/vehicles/get_all", VehiclesGetAll()).RequireAuthorization();

    }


}