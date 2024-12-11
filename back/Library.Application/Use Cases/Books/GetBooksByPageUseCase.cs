using AutoMapper;
using Library.Application.Contracts.Book;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Books
{
    public class GetBooksByPageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetBooksByPageUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ResponseBookDto>> Execute(int page, int pageSize, string userId)
        {
            var UserId = Guid.Parse(userId);
            var books = await _unitOfWork.BookRepository.GetByPage(page, pageSize);
            var booksDto = _mapper.Map<List<ResponseBookDto>>(books);

            var user = await _unitOfWork.UserRepository.GetInfo(UserId);
            if (user == null)
                throw new Exception("Пользователь не найден");

            var userBookIds = user.Books.Select(b => b.Id).ToHashSet();

            foreach (var bookDto in booksDto)
            {
                bookDto.InProfile = userBookIds.Contains(bookDto.Id);
            }

            return booksDto;
        }
    }
}
