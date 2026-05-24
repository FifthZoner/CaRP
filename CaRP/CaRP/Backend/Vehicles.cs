using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CaRP.Shared.Models;
using carp.Shared.Utils;

namespace CaRP.Backend;

public partial class Endpoints
{
    private static async Task<IResult> VehiclesGetAll(ClaimsPrincipal user, [FromServices] CaRpDbContext db)
    {
        if (user.IsNotAtLeast(RoleEnum.Driver))
            return Results.Unauthorized();

        var vehicles = await db.Vehicles.ToListAsync();
        return Results.Ok(vehicles);
    }

    private static async Task<IResult> VehiclesGetDetail(ClaimsPrincipal user, [FromServices] CaRpDbContext db, GetDetailDto getDetailDto, [FromServices] IMapper mapper)
    {
        if (user.IsNotAtLeast(RoleEnum.Manager))
            return Results.Unauthorized();

        var check = Validation.Check(getDetailDto);
        if (check != null)
            return Results.BadRequest(check);

        var vehicle = await db.Vehicles.FindAsync(getDetailDto.Id);

        if (vehicle == null)
            return Results.NotFound();

        return Results.Ok(mapper.Map<VehicleDto>(vehicle));
    }

    private static async Task<IResult> VehiclesAdd(ClaimsPrincipal user, [FromServices] CaRpDbContext db, VehicleDto dto, [FromServices] IMapper mapper)
    {
        if (user.IsNotAtLeast(RoleEnum.Manager))
            return Results.Unauthorized();

        var check = Validation.Check(dto, true);
        if (check != null)
            return Results.BadRequest(check);

        var vehicle = mapper.Map<Vehicle>(dto);

        if (vehicle == null)
            return Results.BadRequest();

        vehicle.Id = 0;

        db.Vehicles.Add(vehicle);
        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Added", Id = vehicle.Id });
    }

    private static async Task<IResult> VehiclesEdit(ClaimsPrincipal user, [FromServices] CaRpDbContext db, VehicleDto dto, [FromServices] IMapper mapper)
    {
        if (user.IsNotAtLeast(RoleEnum.Manager))
            return Results.Unauthorized();

        var check = Validation.Check(dto);
        if (check != null)
            return Results.BadRequest(check);

        var vehicle = await db.Vehicles.FindAsync(dto.Id);
        if (vehicle == null)
            return Results.NotFound();

        mapper.Map(dto, vehicle);

        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Modified", Id = vehicle.Id });
    }


    private static async Task<IResult> VehiclesDelete(ClaimsPrincipal user, [FromServices] CaRpDbContext db, GetDetailDto getDetailDto)
    {
        if (user.IsNotAtLeast(RoleEnum.Manager))
            return Results.Unauthorized();

        var check = Validation.Check(getDetailDto);
        if (check != null)
            return Results.BadRequest(check);

        var vehicle = await db.Vehicles.FindAsync(getDetailDto.Id);

        if (vehicle == null)
            return Results.NotFound();

        db.Vehicles.Remove(vehicle);
        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Deleted", Id = getDetailDto.Id });
    }
}