using System;
using System.Collections.Generic;

namespace CaRP.Shared.Models;

public class Vehicle
{
    public int Id { get; set; }

    public string Vin { get; set; } = null!;

    public string RegistrationNumber { get; set; } = null!;

    public DateOnly? AvailableFrom { get; set; }

    public DateOnly? AvailableTo { get; set; }

    public bool IsOwnedByCompany { get; set; }

    public string VehicleType { get; set; } = null!;

    public virtual ICollection<Servicing> Servicings { get; set; } = new List<Servicing>();

    public virtual ICollection<WorkRegistration> WorkRegistrations { get; set; } = new List<WorkRegistration>();
}
