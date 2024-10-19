using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Books
{
    public class GetBooksCountUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBooksCountUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Execute()
        {
            var count = await _unitOfWork.BookRepository.GetCount();
            return count;
        }
    }
}
