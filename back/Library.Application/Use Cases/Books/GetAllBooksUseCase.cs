using AutoMapper;
using Library.Application.Contracts.Book;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Books
{
    public class GetAllBooksUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllBooksUseCase(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;   
        }
        public async Task<List<ResponseBookDto>> Execute()
        {
            var books = await _unitOfWork.BookRepository.Get();
            var booksDto = _mapper.Map<List<ResponseBookDto>>(books);
            return booksDto;
        }
    }
}
