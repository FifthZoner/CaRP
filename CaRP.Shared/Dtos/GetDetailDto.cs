using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;

public class GetDetailDto
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    [ActualName("Id rekordu")]
    public required int Id { get; set; }
}

public class GetClerkDetailDto
{
    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Id w Clerk")]
    public required string Id { get; set; }
}