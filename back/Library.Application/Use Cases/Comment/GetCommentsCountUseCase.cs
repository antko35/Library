using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Comment
{
    public class GetCommentsCountUseCase
    {
        private readonly IUnitOfWork _unitOfWork; 
        public GetCommentsCountUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Execute(Guid bookId)
        {
            var count = _unitOfWork.CommentRepository.GetCount(bookId);
            return count;
        }
    }
}
