using AutoMapper;
using Library.Core.Abstractions.IInfrastructure;
using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions.IService;
using Library.Core.Contracts.Book;
using Library.Persistence.Entities;
using Library.Persistence.UnitOfWork;
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
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public BookService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _booksRepository = unitOfWork.BookRepository;
            _genreRepository = unitOfWork.GenreRepository;
            _authorRepository = unitOfWork.AuthorRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<List<ResponseBookDto>> GetAll()
        {
            var books = await _booksRepository.Get();
            var booksDto = _mapper.Map<List<ResponseBookDto>>(books);
            return booksDto;
        }

        public async Task<List<ResponseBookDto>> GetByPage(int page, int pageSize)
        {
            var books = await _booksRepository.GetByPage(page, pageSize);
            var booksDto = _mapper.Map<List<ResponseBookDto>>(books);
            return booksDto;
        }

        public async Task<ResponseBookDto> GetById(Guid id)
        { 
            var bookEntity = await _booksRepository.GetByID(id) ?? throw new Exception("Book doesn't exist");

            var bookResponse = _mapper.Map<ResponseBookDto>(bookEntity);
            return bookResponse;
        }

        public async Task<ResponseBookDto> GetByIsbn(string isbn)
        {
            var bookEntity = await _booksRepository.GetByISBN(isbn);
            if (bookEntity == null)
            {
                throw new Exception("Book doesn't exist");
            }
            var bookResponse = _mapper.Map<ResponseBookDto>(bookEntity);
            return bookResponse;
        }
        public async Task<int> GetBooksCount()
        {
            var count = await _booksRepository.GetCount();
            return count;
        }

        public async Task<ResponseBookDto?> Create(RequestBookDto requestBookDto)
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

        public async Task<ResponseBookDto> Update(RequestUpdateBookDto requestBookDto)
        {
            var book = await _booksRepository.GetByID(requestBookDto.Id);
            if(book == null)
            {
                throw new Exception("Book does not exist");
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

            var bookEntityUpd = _mapper.Map(requestBookDto, book);

            _booksRepository.Update(bookEntityUpd);
            await _unitOfWork.Save();
              
            var updatedBook = await _booksRepository.GetByID(book.Id);

            var bookResponse = _mapper.Map<ResponseBookDto>(updatedBook);

            return bookResponse;
        }

        public async Task BorrowBook(Guid bookId, Guid userId)
        {
            var book = await _booksRepository.GetByID(bookId);
            if(book == null)
            {
                throw new Exception("Book doesnt exisi");
            }

            await _booksRepository.BorrowBook(bookId, userId);
        }

        public async Task ReturnBook(Guid bookId, Guid userId)
        {
            var book = await _booksRepository.GetByID(bookId);
            if (book == null)
            {
                throw new Exception("Book doesnt exisi");
            }
            if(book.UserId == null)
            {
                throw new Exception("Book wasnt borrowed");
            }
            if(book.UserId != userId)
            {
                throw new Exception("This user did not take this book");
            }

            await _booksRepository.ReturnBook(bookId);
        }

        public async Task UploadCover(Guid bookId, IFormFile file)
        { 
            var book = await _booksRepository.GetByID(bookId);
            if(book == null)
            {
                throw new Exception("Book doesnt exist");
            }

            var fileName = await _imageService.UploadCover(book, file);
            await _booksRepository.UploadCover(book.Id, fileName);
        }

        public async Task Delete(Guid id)
        {
            var book = _booksRepository.GetByID(id);
            if (book == null)
            {
                throw new Exception("book doesnt exist");
            }
            await _booksRepository.Delete(id);
            await _unitOfWork.Save();
        }
    }
}
