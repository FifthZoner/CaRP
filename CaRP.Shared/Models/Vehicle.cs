using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using carp.Shared.Utils;

namespace CaRP.Shared.Models;

public class Vehicle
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    [ActualName("Id pojazdu")]
    public int Id { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Nr VIN")]
    public string Vin { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Nr rejestracyjny")]
    public string RegistrationNumber { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Dostępny od")]
    public DateOnly? AvailableFrom { get; set; }

    [ValidationMethod(typeof(ValidatorOptional))]
    [ActualName("Dostępny do")]
    public DateOnly? AvailableTo { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Należy do firmy")]
    public bool IsOwnedByCompany { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Typ pojazdu")]
    public string VehicleType { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Servicing> Servicings { get; set; } = new List<Servicing>();
    [JsonIgnore]
    public virtual ICollection<WorkRegistration> WorkRegistrations { get; set; } = new List<WorkRegistration>();
}
