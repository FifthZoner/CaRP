using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CaRP.Shared.Models;

public class WorkRegistration
{
    public int Id { get; set; }

    public string ClerkUsername { get; set; } = null!;

    public int VehicleId { get; set; }

    public DateTime WorkDate { get; set; }

    public decimal DurationHours { get; set; }

    public string Description { get; set; } = null!;

    public decimal CostPerHour { get; set; }
    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
