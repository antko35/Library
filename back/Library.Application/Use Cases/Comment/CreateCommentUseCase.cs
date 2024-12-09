using Library.Application.Contracts.Comment;
using Library.Core.Abstractions;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Comment
{
    public class CreateCommentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(RequestCreateCommentDto commentToCreate, string userId)
        {
            var UserId = Guid.Parse(userId);

            var book = await  _unitOfWork.BookRepository.GetByID(commentToCreate.BookId)
                ?? throw new InvalidOperationException("Book doesnt exist");

            var user = await _unitOfWork.UserRepository.GetByID(UserId)
                ?? throw new InvalidOperationException("problems with user");

            var newComment = new CommentEntity
            {
                Id = Guid.NewGuid(),
                
                BookId = commentToCreate.BookId,
                UserId  = UserId,
                Rate = commentToCreate.Rate,
                Comment = commentToCreate.Comment,
                WritingDate = DateTime.UtcNow,
            };
            await _unitOfWork.CommentRepository.Insert(newComment);
            await _unitOfWork.Save();
        }
    }
}
