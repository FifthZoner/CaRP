using System;
using System.Collections.Generic;

namespace CaRP.Shared.Models;

public partial class Servicing
{
    public int Id { get; set; }

    public int? VehicleId { get; set; }

    public string ClerkUsername { get; set; } = null!;

    public string IssueDescription { get; set; } = null!;

    public DateOnly ServiceDate { get; set; }

    public string? MechanicName { get; set; }

    public decimal Cost { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
