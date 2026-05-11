using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaRP.Backend;

public partial class Endpoints
{
    private static Func<ClaimsPrincipal, CaRpDbContext, Task<IResult>> VehiclesGetAll()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db) =>
        {
            var vehicles = await db.Vehicles.ToListAsync();
            return Results.Ok(vehicles);
        };
    }
}