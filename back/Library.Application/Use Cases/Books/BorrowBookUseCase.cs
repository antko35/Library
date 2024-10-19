using AutoMapper;
using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Library.Application.Use_Cases.Books
{
    public class BorrowBookUseCase
    {
        private readonly IBooksRepository _booksRepository;
        public BorrowBookUseCase( IUnitOfWork unitOfWork)
        {
            _booksRepository = unitOfWork.BookRepository;
        }

        public async Task Execute(Guid bookId, Guid userId)
        {
            var book = await _booksRepository.GetByID(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book doesnt exist");
            }

            await _booksRepository.BorrowBook(bookId, userId);
        }
    }
}
