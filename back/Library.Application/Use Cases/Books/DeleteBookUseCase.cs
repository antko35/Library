using Library.Core.Abstractions;
using Library.Core.Abstractions.IInfrastructure;
using Library.Core.Abstractions.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Books
{
    public class DeleteBookUseCase
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        public DeleteBookUseCase(IUnitOfWork unitOfWork,IImageService imageService)
        {
            _booksRepository = unitOfWork.BookRepository;
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task Execute(Guid id)
        {
            var book = await _booksRepository.GetByID(id);
            if (book == null)
            {
                throw new KeyNotFoundException("book doesn't exist");
            }
            await _booksRepository.Delete(id);
            var cover = book.CoverImagePath;
            _imageService.DeleteCover(cover);
            await _unitOfWork.Save();
        }
    }
}
