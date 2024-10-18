using AutoMapper;
using Library.Application.DTOs.Genre;
using Library.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Mapping
{
    public class MappingGenres : Profile
    {
        public MappingGenres()
        {
            CreateMap<GenreEntity, ResponseGenreDto>();

            CreateMap<RequestGenreDto, GenreEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
