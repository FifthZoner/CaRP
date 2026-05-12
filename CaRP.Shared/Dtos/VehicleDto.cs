using System;
using System.Collections.Generic;
using CaRP.Shared.Models;

namespace CaRP.Shared.Dtos;

public class VehicleDto : Vehicle
{
    public int Id { get; set; }
    public string Vin { get; set; } = null!;
    public string RegistrationNumber { get; set; } = null!;
    public DateOnly? AvailableFrom { get; set; }
    public DateOnly? AvailableTo { get; set; }
    public bool IsOwnedByCompany { get; set; }
    public string VehicleType { get; set; } = null!;
}
