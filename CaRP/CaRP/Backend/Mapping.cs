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
            CreateMap<Vehicle, VehicleDto>()
                .ReverseMap();

            CreateMap<WorkRegistration, WorkRegistrationDto>()
                .ForMember(dest => dest.RegistrationNumber,
                    opt => opt.MapFrom(src => src.Vehicle.RegistrationNumber))
                .ForMember(dest => dest.Vin,
                    opt => opt.MapFrom(src => src.Vehicle.Vin))
                .ReverseMap();

            CreateMap<Servicing, ServicingDto>()
                .ForMember(dest => dest.RegistrationNumber,
                    opt => opt.MapFrom(src => src.Vehicle.RegistrationNumber))
                .ForMember(dest => dest.Vin,
                    opt => opt.MapFrom(src => src.Vehicle.Vin))
                .ReverseMap();
        }
    }
}