﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.Contracts.Book;
using Library.Core.Entities;

namespace Library.Application.Mapping
{
    public class MappingBook : Profile
    {
        public MappingBook() {

            CreateMap<BookEntity, ResponseBookDto>().ReverseMap();
            CreateMap<BookEntity, RequestUpdateBookDto>().ReverseMap();

            CreateMap<RequestBookDto, BookEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
               /* .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => DateTime.UtcNow))  // Текущая дата для BorrowDate
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddDays(7)));  // ReturnDate через неделю*/
        }
    }
}
