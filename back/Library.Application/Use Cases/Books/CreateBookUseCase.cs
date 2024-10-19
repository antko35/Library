using AutoMapper;
using Library.Application.Contracts.Book;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Books
{
    public class CreateBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public CreateBookUseCase(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _booksRepository=unitOfWork.BookRepository;
            _authorRepository=unitOfWork.AuthorRepository;
            _genreRepository=unitOfWork.GenreRepository;
            _mapper=mapper;
            _unitOfWork=unitOfWork;
        }
        public async Task<ResponseBookDto> Execute(RequestBookDto requestBookDto)
        {
            var book = await _booksRepository.GetByISBN(requestBookDto.ISBN);
            if (book != null)
            {
                throw new Exception("Book already exist");
            }

            var genre = await _genreRepository.GetByID(requestBookDto.GenreId);
            if (genre == null)
            {
                throw new Exception("Genre does not exist");
            }

            var author = await _authorRepository.GetByID(requestBookDto.AuthorId);
            if (author == null)
            {
                throw new Exception("Author does not exist");
            }

            var bookToCreate = _mapper.Map<BookEntity>(requestBookDto);
            await _booksRepository.Insert(bookToCreate);
            await _unitOfWork.Save();

            var createdBook = await _booksRepository.GetByID(bookToCreate.Id);
            var bookResponse = _mapper.Map<ResponseBookDto>(createdBook);
            return bookResponse;
        }
    }
}
