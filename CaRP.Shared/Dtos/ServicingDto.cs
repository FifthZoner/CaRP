using System;
using System.Collections.Generic;
using CaRP.Shared.Models;
using carp.Shared.Utils;

namespace CaRP.Shared.Dtos;

public class ServicingDto : Servicing
{
    [ValidationMethod(typeof(ValidatorOptional))]
    public string Vin { get; set; } = null!;

    [ValidationMethod(typeof(ValidatorNotNull))]
    public string RegistrationNumber { get; set; } = null!;
}
