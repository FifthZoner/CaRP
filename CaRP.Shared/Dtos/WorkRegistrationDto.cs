using System;
using System.Collections.Generic;
using CaRP.Shared.Models;

namespace CaRP.Shared.Dtos;

public class WorkRegistrationDto : WorkRegistration
{
    public string Vin { get; set; } = null!;

    public string? RegistrationNumber { get; set; } = null!;
}
