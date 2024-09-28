using AutoMapper;
using FluentValidation;
using Library.Core.Contracts.User;
using Library.Core.Entities;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Mapping
{
    public class MappingUser : Profile
    {
        public MappingUser()
        {
            CreateMap<LoginRequestUserDto, UserEntity>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ReverseMap();

            CreateMap<RegisterRequestDto, UserEntity>()
                 .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                 .ReverseMap();
        }
    }
}
