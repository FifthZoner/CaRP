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

        api.MapGet("/user/login_details",   LoginDetails)     .RequireAuthorization();

        api.MapGet ("/vehicles/get_all",    VehiclesGetAll)   .RequireAuthorization();
        api.MapPost("/vehicles/get_detail", VehiclesGetDetail).RequireAuthorization();
        api.MapPost("/vehicles/add",        VehiclesAdd)      .RequireAuthorization();
        api.MapPost("/vehicles/edit",       VehiclesEdit)     .RequireAuthorization();
        api.MapPost("/vehicles/delete",     VehiclesDelete)   .RequireAuthorization();

        api.MapGet ("/work/get_all",        WorkGetAll)       .RequireAuthorization();
        api.MapPost("/work/get_detail",     WorkGetDetail)    .RequireAuthorization();
        api.MapPost("/work/add",            WorkAdd)          .RequireAuthorization();
        api.MapPost("/work/edit",           WorkEdit)         .RequireAuthorization();
        api.MapPost("/work/delete",         WorkDelete)       .RequireAuthorization();

        api.MapGet ("/service/get_all",     ServiceGetAll)    .RequireAuthorization();
        api.MapPost("/service/get_detail",  ServiceGetDetail) .RequireAuthorization();
        api.MapPost("/service/add",         ServiceAdd)       .RequireAuthorization();
        api.MapPost("/service/edit",        ServiceEdit)      .RequireAuthorization();
        api.MapPost("/service/delete",      ServiceDelete)    .RequireAuthorization();
    }


}