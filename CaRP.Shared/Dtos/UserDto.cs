using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;
using carp.Shared.Enums;

public class UserDto
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    public string Id { get; set; } = string.Empty;
    [ValidationMethod(typeof(ValidatorNotNull))]
    public string Username { get; set; } = string.Empty;
    [ValidationMethod(typeof(ValidatorOptional))]
    public string? FirstName { get; set; }
    [ValidationMethod(typeof(ValidatorOptional))]
    public string? LastName { get; set; }
    [ValidationMethod(typeof(ValidatorOptional))]
    public string Email { get; set; } = string.Empty;
    [ValidationMethod(typeof(ValidatorNotNull))]
    public RoleEnum RoleLevel { get; set; }
}


