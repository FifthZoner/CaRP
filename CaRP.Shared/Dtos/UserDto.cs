using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;
using carp.Shared.Enums;

public class UserDto
{
    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Id użytkownika")]
    public string Id { get; set; } = string.Empty;

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Login")]
    public string Username { get; set; } = string.Empty;

    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Imie")]
    public string? FirstName { get; set; }

    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Nazwisko")]
    public string? LastName { get; set; }

    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Email")]
    public string Email { get; set; } = string.Empty;

    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Link do obrazka")]
    public string ImageUrl { get; set; } = string.Empty;

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Rola")]
    public RoleEnum RoleLevel { get; set; } = RoleEnum.Unset;
}


