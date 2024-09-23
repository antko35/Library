using AutoMapper;
using Library.Core.Contracts.Book;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Servises
{
    public class BookService : IBookService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IMapper _mapper;
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;
        public BookService(IBooksRepository booksRepository,IGenreRepository genreRepository,IAuthorRepository authorRepository, IMapper mapper)
        {
            _booksRepository = booksRepository;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<List<ResponseBookDto>> GetAll()
        {
            var books = await _booksRepository.GetAll();
            var booksDto = _mapper.Map<List<ResponseBookDto>>(books);
            return booksDto;
        }

        public async Task<ResponseBookDto> GetById(Guid id)
        { 
            var bookEntity = await _booksRepository.GetById(id) ?? throw new Exception("Book doesn't exist");

            var bookResponse = _mapper.Map<ResponseBookDto>(bookEntity);
            return bookResponse;
        }

        public async Task<ResponseBookDto?> Create(RequestBookDto requestBookDto)
        {
            var alreadyExist = await _booksRepository.AlreadyExist(requestBookDto.ISBN);
            if (alreadyExist == null)
            {
                bool genre = await _genreRepository.IsExist(requestBookDto.GenreId);
                if (!genre)
                {
                    throw new Exception("Genre does not exist");
                }

                bool author = await _authorRepository.IsExist(requestBookDto.AuthorId);
                if (!author)
                {
                    throw new Exception("Author does not exist");
                }

                var bookToCreate = _mapper.Map<BookEntity>(requestBookDto);
                await _booksRepository.Create(bookToCreate);
                var bookResponse = _mapper.Map<ResponseBookDto>(bookToCreate);
                return bookResponse;
            }
            else
            {
                throw new Exception("already exist");
            }
        }
    }
}
