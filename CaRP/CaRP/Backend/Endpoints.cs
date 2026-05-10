using CaRP.Shared.Dtos;

namespace CaRP.Backend;

public static partial class Endpoints
{
    public static void MapEndpoints(RouteGroupBuilder api)
    {
        // testing
        api.MapGet("/test", () => Results.Ok(new { Message = "Backend works!" }));

        api.MapPost("/user/login", Login);
    }


}