using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Library.Core.Abstractions.IInfrastructure;

namespace Library.Application.Use_Cases.Books
{
    public class UploadCoverUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBooksRepository _booksRepository;
        private readonly IImageService _imageService;
        public UploadCoverUseCase(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _booksRepository = unitOfWork.BookRepository;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task Execute(Guid bookId, IFormFile file)
        {
            var book = await _booksRepository.GetByID(bookId);
            if (book == null)
            {
                throw new Exception("Book doesnt exist");
            }

            var fileName = await _imageService.UploadCover(book, file);
            await _booksRepository.UploadCover(book.Id, fileName);
        }
    }
}
