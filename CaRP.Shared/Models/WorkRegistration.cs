using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using carp.Shared.Utils;

namespace CaRP.Shared.Models;

public class WorkRegistration
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    public int Id { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string ClerkUsername { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    public int VehicleId { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public DateTime WorkDate { get; set; }

    [ValidationMethod(typeof(ValidatorGreaterThan0<decimal>))]
    public decimal DurationHours { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string Description { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<decimal>))]
    public decimal CostPerHour { get; set; }
    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
