using carp.Shared.Enums;

namespace CaRP.Shared.Dtos;

public class LoginInfoDto
{
    public required string Username { get; set; }
    public string? ImageUrl { get; set; }
    public required RoleEnum RoleLevel { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}