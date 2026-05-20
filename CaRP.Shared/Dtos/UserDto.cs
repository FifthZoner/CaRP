namespace CaRP.Shared.Dtos;
using carp.Shared.Enums;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public RoleEnum RoleLevel { get; set; }
}


