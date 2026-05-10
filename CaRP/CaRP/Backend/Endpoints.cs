using System.Security.Claims;
using CaRP.Shared.Dtos;
using Clerk.Net.Client;

namespace CaRP.Backend;

public static partial class Endpoints
{
    public static void MapEndpoints(RouteGroupBuilder api)
    {
        // testing
        api.MapGet("/test", () => Results.Ok(new { Message = "Backend works!" }));

         api.MapGet("/user/check_login", async () =>
         {
             int k = 10;

             return Results.Ok(new {
                 Name = "idk"
             });
         }).RequireAuthorization();
    }


}