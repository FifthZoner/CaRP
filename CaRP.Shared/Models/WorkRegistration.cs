using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using carp.Shared.Utils;

namespace CaRP.Shared.Models;

public class WorkRegistration
{
    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    [ActualName("Id karty pracy")]
    public int Id { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Login")]
    public string ClerkUsername { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<int>))]
    [ActualName("Id pojazdu")]
    public int VehicleId { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Data pracy")]
    public DateTime WorkDate { get; set; }

    [ValidationMethod(typeof(ValidatorGreaterThan0<decimal>))]
    [ActualName("Ilość godzin")]
    public decimal DurationHours { get; set; }

    [ValidationMethod(typeof(ValidatorNotNull))]
    [ActualName("Opis")]
    public string Description { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorGreaterThan0<decimal>))]
    [ActualName("Koszt godziny")]
    public decimal CostPerHour { get; set; }
    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
