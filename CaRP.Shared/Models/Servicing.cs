using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using carp.Shared.Utils;

namespace CaRP.Shared.Models;

public class Servicing
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    [ActualName("Id serwisu")]
    public int Id { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Nr serwisu")]
    public string ServiceNumber { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    [ActualName("Id pojazdu")]
    public int VehicleId { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Login")]
    public string ClerkUsername { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Opis problemu")]
    public string IssueDescription { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Data serwisu")]
    public DateOnly ServiceDate { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Mechanik")]
    public string MechanicName { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<decimal>))]
    [ActualName("Koszt")]
    public decimal Cost { get; set; }
    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
