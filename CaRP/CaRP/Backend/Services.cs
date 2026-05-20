using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CaRP.Shared.Models;
using carp.Shared.Permissions;

namespace CaRP.Backend;

public partial class Endpoints
{
    private static async Task<IResult> ServiceGetAll(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromServices] IMapper mapper)
    {
        if (user.Has(Perms.Services.CanReadAll))
            return Results.Ok(await db.Services
                .ProjectTo<ServicingDto>(mapper.ConfigurationProvider)
                .ToListAsync());
        if (user.Has(Perms.Services.CanReadOwn))
            return Results.Ok(await db.Services
                .Where(x => x.ClerkUsername == user.Login())
                .ProjectTo<ServicingDto>(mapper.ConfigurationProvider)
                .ToListAsync());
        return Results.Unauthorized();
    }

    private static async Task<IResult> ServiceGetDetail(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] GetDetailDto getDetailDto, [FromServices] IMapper mapper)
    {
        var dto = await db.Services
            .ProjectTo<ServicingDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == getDetailDto.Id);

        if (dto == null)
            return Results.NotFound();

        if (!user.CanRead(Perms.Services, dto.ClerkUsername))
            return Results.Unauthorized();

        return Results.Ok(dto);
    }

    private static async Task<IResult> ServiceAdd(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] ServicingDto dto, [FromServices] IMapper mapper)
    {
        if (!user.CanWrite(Perms.Services, dto.ClerkUsername))
            return Results.Unauthorized();

        var entity = mapper.Map<Servicing>(dto);
        entity.Id = 0;

        var vehicle = await db.Vehicles.FindAsync(dto.VehicleId);
        if (vehicle == null)
            return Results.NotFound("Invalid vehicle id");
        entity.Vehicle = vehicle;

        db.Services.Add(entity);
        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Added", Id = entity.Id });
    }

    private static async Task<IResult> ServiceEdit(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] ServicingDto dto, [FromServices] IMapper mapper)
    {
        var entity = await db.Services.FindAsync(dto.Id);
        if (entity == null) return Results.NotFound();
        mapper.Map(dto, entity);

        if (!user.CanWrite(Perms.Services, entity.ClerkUsername))
            return Results.Unauthorized();

        var vehicle = await db.Vehicles.FindAsync(dto.VehicleId);
        if (vehicle == null)
            return Results.NotFound();
        entity.Vehicle = vehicle;

        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Modified", Id = entity.Id });
    }

    private static async Task<IResult> ServiceDelete(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] GetDetailDto getDetailDto)
    {
        var entity = await db.Services.FindAsync(getDetailDto.Id);
        if (entity == null)
            return Results.NotFound();

        if (!user.CanWrite(Perms.Services, entity.ClerkUsername))
            return Results.Unauthorized();

        db.Services.Remove(entity);
        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Deleted", Id = getDetailDto.Id });
    }
}