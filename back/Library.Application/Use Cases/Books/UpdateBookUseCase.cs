using AutoMapper;
using Library.Application.Contracts.Book;
using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions;

namespace Library.Application.Use_Cases.Books
{
    public class UpdateBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public UpdateBookUseCase(IMapper mapper, IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _booksRepository = unitOfWork.BookRepository;
            _authorRepository = unitOfWork.AuthorRepository;
            _genreRepository = unitOfWork.GenreRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }
        public async Task<ResponseBookDto> Execute(RequestUpdateBookDto requestBookDto)
        {
            await _validationService.ValidateAsync(requestBookDto);
            var book = await _booksRepository.GetByID(requestBookDto.Id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book does not exist");
            }

            var genre = await _genreRepository.GetByID(requestBookDto.GenreId);
            if (genre == null)
            {
                throw new KeyNotFoundException("Genre does not exist");
            }

            var author = await _authorRepository.GetByID(requestBookDto.AuthorId);
            if (author == null)
            {
                throw new KeyNotFoundException("Author does not exist");
            }

            var bookEntityUpd = _mapper.Map(requestBookDto, book);

            _booksRepository.Update(bookEntityUpd);
            await _unitOfWork.Save();

            var updatedBook = await _booksRepository.GetByID(book.Id);

            var bookResponse = _mapper.Map<ResponseBookDto>(updatedBook);

            return bookResponse;
        }
    }
}
