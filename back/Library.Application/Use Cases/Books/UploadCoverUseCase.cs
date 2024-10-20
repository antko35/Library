using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Library.Core.Abstractions.IInfrastructure;
using Library.Application.Contracts.Book;

namespace Library.Application.Use_Cases.Books
{
    public class UploadCoverUseCase
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IImageService _imageService;
        private readonly IValidationService _validationService;
        public UploadCoverUseCase(IUnitOfWork unitOfWork, IImageService imageService, IValidationService validationService)
        {
            _booksRepository = unitOfWork.BookRepository;
            _imageService = imageService;
            _validationService = validationService;
        }
        public async Task Execute(RequestUploadCoverDto request)
        {
            await _validationService.ValidateAsync(request);
            var book = await _booksRepository.GetByID(request.BookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book doesn't exist");
            }

            var fileName = await _imageService.UploadCover(book, request.File);
            await _booksRepository.UploadCover(book.Id, fileName);
        }
    }
}
