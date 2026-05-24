using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;

public class GetDetailDto
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    public required int Id { get; set; }
}

public class GetClerkDetailDto
{
    [ValidationMethod(typeof(ValidatorNotNull))]
    public required string Id { get; set; }
}