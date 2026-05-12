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
    private static Func<ClaimsPrincipal, CaRpDbContext, IMapper, Task<IResult>> WorkGetAll()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromServices] IMapper mapper) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Driver))
                return Results.Unauthorized();

            var work = await db.WorkRegistrations.Include(x => x.Vehicle).ToListAsync();
            return Results.Ok(mapper.Map<List<WorkRegistrationDto>>(work));
        };
    }

    private static Func<ClaimsPrincipal, CaRpDbContext, GetDetailDto, IMapper, Task<IResult>> WorkGetDetail()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] GetDetailDto getDetailDto, [FromServices] IMapper mapper) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Driver))
                return Results.Unauthorized();

            var work = await db.WorkRegistrations
                .Include(x => x.Vehicle)
                .FirstOrDefaultAsync(x => x.Id == getDetailDto.Id);

            if (work == null)
                return Results.NotFound();

            return Results.Ok(mapper.Map<WorkRegistrationDto>(work));
        };
    }

    private static Func<ClaimsPrincipal, CaRpDbContext, WorkRegistrationDto, IMapper, Task<IResult>> WorkAdd()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] WorkRegistrationDto dto, [FromServices] IMapper mapper) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Driver))
                return Results.Unauthorized();

            var work = mapper.Map<WorkRegistration>(dto);
            work.Id = 0;

            db.WorkRegistrations.Add(work);
            await db.SaveChangesAsync();

            return Results.Ok(new { Message = "Added", Id = work.Id });
        };
    }

    private static Func<ClaimsPrincipal, CaRpDbContext, WorkRegistrationDto, IMapper, Task<IResult>> WorkEdit()
    {
        return async (ClaimsPrincipal user, [FromServices] CaRpDbContext db, [FromBody] WorkRegistrationDto dto, [FromServices] IMapper mapper) =>
        {
            if (user.IsNotAtLeast(RoleEnum.Driver))
                return Results.Unauthorized();

            var work = await db.WorkRegistrations.FindAsync(dto.Id);
            if (work == null) return Results.NotFound();

            mapper.Map(dto, work);
            await db.SaveChangesAsync();

            return Results.Ok(new { Message = "Modified", Id = work.Id });
        };
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