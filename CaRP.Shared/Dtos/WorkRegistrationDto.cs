using CaRP.Shared.Models;
using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;

public class WorkRegistrationDto : WorkRegistration
{
    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Nr VIN")]
    public string Vin { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Nr rejestracyjny")]
    public string RegistrationNumber { get; set; } = null!;
}
