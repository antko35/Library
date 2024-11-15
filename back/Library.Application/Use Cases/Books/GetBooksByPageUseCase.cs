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
    public class GetBooksByPageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetBooksByPageUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ResponseBookDto>> Execute(int page, int pageSize, string author, string genre)
        {
            var books = await _unitOfWork.BookRepository.GetByPage(page, pageSize);
            var booksDto = _mapper.Map<List<ResponseBookDto>>(books);
            return booksDto;
        }
    }
}
