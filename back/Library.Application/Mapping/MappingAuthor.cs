using AutoMapper;
using Library.Application.Contracts.Author;
using Library.Core.Entities;


namespace Library.Application.Mapping
{
    public class MappingAuthor : Profile
    {
        public MappingAuthor()
        {
             CreateMap<RequestAuthorDto,AuthorEntity>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<AuthorEntity, ResponseAuthorDto>().ReverseMap();

            CreateMap<RequestUpdateAuthorDto, AuthorEntity>();
        }
       
    }
}
