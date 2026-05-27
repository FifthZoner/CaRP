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
    private static async Task<IResult> WorkGetAll(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromServices] IMapper mapper)
    {
        if (user.IsNotAtLeast(RoleEnum.Driver))
            return Results.Unauthorized();

        List<WorkRegistrationDto> work;

        if (user.Has(Perms.Work.CanReadAll))
            work = await db.WorkRegistrations
                .ProjectTo<WorkRegistrationDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        else if (user.Has(Perms.Work.CanReadOwn))
            work = await db.WorkRegistrations
                .Where(x => x.ClerkUsername == user.Login())
                .ProjectTo<WorkRegistrationDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        else return Results.Unauthorized();

        return Results.Ok(work.OrderBy(x => x.WorkDate).ThenBy(x => x.RegistrationNumber).ToList());
    }

    private static async Task<IResult> WorkGetDetail(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] GetDetailDto getDetailDto, [FromServices] IMapper mapper)
    {
        if (user.IsNotAtLeast(RoleEnum.Driver))
            return Results.Unauthorized();

        var work = await db.WorkRegistrations
            .ProjectTo<WorkRegistrationDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == getDetailDto.Id);

        if (work == null)
            return Results.NotFound();

        if (!user.Has(Perms.Work.CanReadAll))
            if (!user.Has(Perms.Work.CanReadOwn) || work.ClerkUsername != user.Login())
                return Results.Unauthorized();

        return Results.Ok(work);
    }

    private static async Task<IResult> WorkAdd(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] WorkRegistrationDto dto, [FromServices] IMapper mapper)
    {
        if (user.IsNotAtLeast(RoleEnum.Driver))
            return Results.Unauthorized();


        var work = mapper.Map<WorkRegistration>(dto);
        work.Id = 0;

        var vehicle = await db.Vehicles.FindAsync(dto.VehicleId);
        if (vehicle == null)
            return Results.NotFound();
        work.Vehicle = vehicle;

        work.WorkDate = DateTime.SpecifyKind(work.WorkDate ,DateTimeKind.Utc);

        db.WorkRegistrations.Add(work);
        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Added", Id = work.Id });
    }

    private static async Task<IResult> WorkEdit(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] WorkRegistrationDto dto, [FromServices] IMapper mapper)
    {
        if (user.IsNotAtLeast(RoleEnum.Driver))
            return Results.Unauthorized();

        var work = await db.WorkRegistrations.FindAsync(dto.Id);
        if (work == null) return Results.NotFound();
        mapper.Map(dto, work);

        var vehicle = await db.Vehicles.FindAsync(dto.VehicleId);
        if (vehicle == null)
            return Results.NotFound();
        work.Vehicle = vehicle;

        work.WorkDate = DateTime.SpecifyKind(work.WorkDate ,DateTimeKind.Utc);

        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Modified", Id = work.Id });
    }

    private static async Task<IResult> WorkDelete(ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] GetDetailDto getDetailDto)
    {
        if (user.IsNotAtLeast(RoleEnum.Manager))
            return Results.Unauthorized();

        var work = await db.WorkRegistrations.FindAsync(getDetailDto.Id);
        if (work == null) return Results.NotFound();

        db.WorkRegistrations.Remove(work);
        await db.SaveChangesAsync();

        return Results.Ok(new { Message = "Deleted", Id = getDetailDto.Id });
    }
}