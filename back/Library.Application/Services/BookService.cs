using AutoMapper;
using Library.Core.Contracts.Book;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
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

        public async Task<ResponseBookDto> GetByIsbn(string isbn)
        {
            var bookEntity = await _booksRepository.AlreadyExist(isbn);
            if (bookEntity == null)
            {
                throw new Exception("Book doesn't exist");
            }
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
                throw new Exception("Book already exist");
            }
        }

        public async Task<ResponseBookDto> Update(RequestBookDto requestBookDto)
        {
            var existing = await _booksRepository.AlreadyExist(requestBookDto.ISBN);
            if(existing == null)
            {
                throw new Exception("Book does not exist");
            }

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

            var bookEntityUpd= _mapper.Map<BookEntity>(requestBookDto);

            await _booksRepository.Update(existing.Id , bookEntityUpd);

            var updatedBook = await _booksRepository.GetById(existing.Id);

            var bookResponse = _mapper.Map<ResponseBookDto>(updatedBook);

            return bookResponse;
        }
        public async Task BorrowBook(Guid bookId)
        {
            var book = _booksRepository.GetById(bookId);
            if(book == null)
            {
                throw new Exception("Book doesnt exisi");
            }
            await _booksRepository.BorrowBook(bookId);
        }
        public async Task UploadCover(Guid bookId, IFormFile file)
        { 
            var book = await _booksRepository.GetById(bookId);
            if(book == null)
            {
                throw new Exception("Book doesnt exist");
            }

            if (!string.IsNullOrEmpty(book.CoverImagePath))
            {
                var oldFilePath = Path.Combine("wwwroot", "uploads", book.CoverImagePath);

                // Удаляем старую фотографию, если она существует
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot", "uploads", fileName);

            // Сохранение файла на сервере
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _booksRepository.UploadCover(book.Id, fileName);
        }

        public async Task Delete(Guid id)
        {
            var deleteCount = await _booksRepository.Delete(id);
            if(deleteCount == 0)
            {
                throw new Exception("Nothing wos deleted");
            }
        }
    }
}
