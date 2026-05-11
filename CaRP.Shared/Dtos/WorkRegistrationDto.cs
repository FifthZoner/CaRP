using System;
using System.Collections.Generic;
using CaRP.Shared.Models;

namespace CaRP.Shared.Dtos;

public class WorkRegistrationDto : WorkRegistration
{
    public string? VehicleRegistrationNumber { get; set; }
    public string? VehicleName { get; set; }
}
