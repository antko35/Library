using Library.Core.Abstractions;
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
        public DeleteBookUseCase(IUnitOfWork unitOfWork)
        {
            _booksRepository = unitOfWork.BookRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(Guid id)
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
