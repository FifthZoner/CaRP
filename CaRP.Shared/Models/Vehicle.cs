using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using carp.Shared.Utils;

namespace CaRP.Shared.Models;

public class Vehicle
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    public int Id { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string Vin { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string RegistrationNumber { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorOptional))]
    public DateOnly? AvailableFrom { get; set; }

    [ValidationMethod(typeof(ValidatorOptional))]
    public DateOnly? AvailableTo { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public bool IsOwnedByCompany { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string VehicleType { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Servicing> Servicings { get; set; } = new List<Servicing>();
    [JsonIgnore]
    public virtual ICollection<WorkRegistration> WorkRegistrations { get; set; } = new List<WorkRegistration>();
}
