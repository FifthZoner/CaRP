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
using Clerk.Net.Client;
using Clerk.Net.Client.Models;
using Clerk.Net.Client.Users.Item;
using Microsoft.Kiota.Abstractions.Serialization;

namespace CaRP.Backend;

public partial class Endpoints
{
    private static async Task<IResult> UsersGetAll(ClaimsPrincipal user, [FromServices] ClerkApiClient clerkClient)
    {
        if (!user.HasAll(Perms.Users.CanReadAll, Perms.Users.CanReadOwn))
            return Results.Unauthorized();

        var users = await clerkClient.Users.GetAsync(config =>
        {
            config.QueryParameters.Limit = 256;
        });

        if (users == null)
            return Results.NotFound();

        return Results.Ok(users.ConvertAll(ClerkUserToDto));
    }

    private static async Task<IResult> UsersGetDetail(ClaimsPrincipal user, [FromBody] GetClerkDetailDto getDetailDto, [FromServices] ClerkApiClient clerkClient)
    {
        if (!user.HasAll(Perms.Users.CanReadAll, Perms.Users.CanReadOwn))
            return Results.Unauthorized();

        var users = await clerkClient.Users.GetAsync(config =>
        {
            config.QueryParameters.UserId = [getDetailDto.Id];
        });

        if (users == null || users.Count != 1)
            return Results.NotFound();

        return Results.Ok(ClerkUserToDto(users.First()));
    }

    private static async Task<IResult> UsersEdit(ClaimsPrincipal user, [FromBody] UserDto dto, [FromServices] ClerkApiClient clerkClient)
    {
        if (!user.HasAll(Perms.Users.CanFullAll, Perms.Users.CanFullOwn))
            return Results.Unauthorized();

        var users = await clerkClient.Users.GetAsync(config =>
        {
            config.QueryParameters.UserId = [dto.Id];
        });

        if (users == null || users.Count != 1)
            return Results.NotFound();

        var metadataPayload = new WithUser_PatchRequestBody()
        {
            PublicMetadata = new WithUser_PatchRequestBody_public_metadata()
            {
                AdditionalData = new Dictionary<string, object>
                {
                    { "role_number", ((int)dto.RoleLevel).ToString() }
                }
            }
        };

        try
        {
            clerkClient.Users[dto.Id].PatchAsync(metadataPayload).Wait();
        }
        catch
        {
            return Results.InternalServerError();
        }

        return Results.Ok(new { Message = "Modified", Id = dto.Id });
    }

    private static UserDto ClerkUserToDto(User u)
    {
        string email = "";
        if (u.EmailAddresses != null) email = u.EmailAddresses.FirstOrDefault()?.EmailAddressProp ?? "";

        string? role = null;
        if (u.PublicMetadata != null)
            if (u.PublicMetadata.AdditionalData.TryGetValue("role_number", out var value))
                if (value is string)
                    role = value as string;

        return new UserDto
        {
            Id = u.Id ?? "",
            Username = u.Username ?? "",
            Email = email,
            FirstName = u.FirstName ?? "",
            LastName = u.LastName ?? "",
            RoleLevel = GetRole(role) ?? RoleEnum.Unset
        };
    }
}