using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using carp.Shared.Utils;

namespace CaRP.Shared.Models;

public class Servicing
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    public int Id { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string ServiceNumber { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    public int VehicleId { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string ClerkUsername { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string IssueDescription { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    public DateOnly ServiceDate { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string MechanicName { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<decimal>))]
    public decimal Cost { get; set; }
    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
