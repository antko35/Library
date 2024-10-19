﻿using AutoMapper;
using Library.Application.Contracts.Book;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Books
{
    public class GetBookByIsbnUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetBookByIsbnUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseBookDto> Execute(string isbn)
        {
            var bookEntity = await _unitOfWork.BookRepository.GetByISBN(isbn);
            if (bookEntity == null)
            {
                throw new Exception("Book doesn't exist");
            }
            var bookResponse = _mapper.Map<ResponseBookDto>(bookEntity);
            return bookResponse;
        }
    }
}
