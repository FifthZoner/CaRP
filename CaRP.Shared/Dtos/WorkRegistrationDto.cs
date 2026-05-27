using CaRP.Shared.Models;
using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;

public class WorkRegistrationDto : WorkRegistration
{
    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Nr VIN")]
    public string Vin { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Nr rejestracyjny")]
    public string RegistrationNumber { get; set; } = null!;
}
