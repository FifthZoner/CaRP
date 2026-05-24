using CaRP.Shared.Models;
using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;

public class WorkRegistrationDto : WorkRegistration
{
    [ValidationMethod(typeof(ValidatorOptional))]
    public string Vin { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string RegistrationNumber { get; set; } = null!;
}
