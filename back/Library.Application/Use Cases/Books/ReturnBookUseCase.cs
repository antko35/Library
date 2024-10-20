﻿using AutoMapper;
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
    public class ReturnBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBooksRepository _booksRepository;
        public ReturnBookUseCase(IUnitOfWork unitOfWork)
        {
            _booksRepository = unitOfWork.BookRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(Guid bookId, Guid userId)
        {
            var book = await _booksRepository.GetByID(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book doesnt exist");
            }
            if (book.UserId == null)
            {
                throw new Exception("Book wasn't borrowed");
            }
            if (book.UserId != userId)
            {
                throw new Exception("This user did not take this book");
            }

            await _booksRepository.ReturnBook(bookId);
        }
    }
}
