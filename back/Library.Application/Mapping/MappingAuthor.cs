using AutoMapper;
using Library.Core.Contracts.Author;
using Library.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Mapping
{
    public class MappingAuthor : Profile
    {
        public MappingAuthor()
        {
             CreateMap<RequestAuthorDto,AuthorEntity>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<AuthorEntity, ResponseAuthorDto>().ReverseMap();
        }
       
    }
}
