using System.Net.Http.Headers;
using System.Security.Claims;
using CaRP.Shared.Dtos;
using carp.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CaRP.Shared.Models;

namespace CaRP.Backend;

public partial class Endpoints
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleDto, Vehicle>();
            CreateMap<WorkRegistration, WorkRegistrationDto>();
            CreateMap<WorkRegistrationDto, WorkRegistration>();
            CreateMap<Servicing, ServicingDto>();
            CreateMap<ServicingDto, Servicing>();
        }
    }
}