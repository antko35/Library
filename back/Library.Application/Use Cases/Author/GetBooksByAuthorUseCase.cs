using AutoMapper;
using Library.Application.Contracts.Book;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Author
{
    public class GetBooksByAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 
        public GetBooksByAuthorUseCase(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ResponseBookDto>> Execute(Guid authorId)
        {
            var authorEntity = await _unitOfWork.AuthorRepository.GetByID(authorId);

            if (authorEntity == null)
            {
                throw new KeyNotFoundException("Author does not exist");
            }

            var books = await _unitOfWork.AuthorRepository.GetBookByAuthor(authorId);

            var booksResponse = books
                .Select(b => _mapper.Map<ResponseBookDto>(b))
                .ToList();

            return booksResponse;
        }
    }
}
