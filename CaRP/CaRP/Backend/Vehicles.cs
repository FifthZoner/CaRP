using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CaRP.Shared.Models;

namespace CaRP.Backend;

public partial class Endpoints
{
    private static Func<ClaimsPrincipal, CaRpDbContext, Task<IResult>> VehiclesGetAll()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Driver))
                return Results.Unauthorized();

            var vehicles = await db.Vehicles.ToListAsync();
            return Results.Ok(vehicles);
        };
    }

    private static Func<ClaimsPrincipal, CaRpDbContext, GetDetailDto, IMapper, Task<IResult>> VehiclesGetDetail()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, GetDetailDto getDetailDto, [FromServices] IMapper mapper) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Manager))
                return Results.Unauthorized();

            var vehicle = await db.Vehicles.FindAsync(getDetailDto.Id);

            if (vehicle == null)
                return Results.NotFound();

            return Results.Ok(mapper.Map<VehicleDto>(vehicle));
        };
    }

    private static Func<ClaimsPrincipal, CaRpDbContext, VehicleDto, IMapper, Task<IResult>> VehiclesAdd()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, VehicleDto dto, [FromServices] IMapper mapper) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Manager))
                return Results.Unauthorized();

            var vehicle = mapper.Map<Vehicle>(dto);

            if (vehicle == null)
                return Results.BadRequest();

            vehicle.Id = 0;

            // TODO: sprawdzenia

            db.Vehicles.Add(vehicle);
            await db.SaveChangesAsync();

            return Results.Ok(new { Message = "Added", Id = vehicle.Id });
        };
    }

    private static Func<ClaimsPrincipal, CaRpDbContext, VehicleDto, IMapper, Task<IResult>> VehiclesEdit()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, VehicleDto dto, [FromServices] IMapper mapper) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Manager))
                return Results.Unauthorized();

            var vehicle = await db.Vehicles.FindAsync(dto.Id);
            if (vehicle == null)
                return Results.NotFound();

            mapper.Map(dto, vehicle);

            // TODO: sprawdzenia

            await db.SaveChangesAsync();

            return Results.Ok(new { Message = "Modified", Id = vehicle.Id });
        };
    }


    private static Func<ClaimsPrincipal, CaRpDbContext, GetDetailDto, Task<IResult>> VehiclesDelete()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, GetDetailDto getDetailDto) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Manager))
                return Results.Unauthorized();

            var vehicle = await db.Vehicles.FindAsync(getDetailDto.Id);

            if (vehicle == null)
                return Results.NotFound();

            db.Vehicles.Remove(vehicle);
            await db.SaveChangesAsync();

            return Results.Ok(new { Message = "Deleted", Id = getDetailDto.Id });
        };
    }
}