using AutoMapper;
using Library.Application.Contracts.Comment;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Mapping
{
    public class MappingComments : Profile
    {
        public MappingComments()
        {
            CreateMap<CommentEntity, ResponseCommentDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(c => c.User.UserName));
        }
    }
}
