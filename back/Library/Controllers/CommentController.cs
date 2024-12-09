using Library.Application.Contracts.Comment;
using Library.Application.Use_Cases.Comment;
using Library.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    //8e855e6e-818c-4a6d-973a-e02d0129bbec
    public class CommentController : ControllerBase
    {
        private readonly GetCommentsByBookUseCase _getCommentsByBookUseCase;
        private readonly GetCommentsCountUseCase _getCommentsCountUseCase;
        public CommentController(
            GetCommentsByBookUseCase getCommentsByBook,
            GetCommentsCountUseCase getCommentsCountUseCase

            )
        {
            _getCommentsByBookUseCase = getCommentsByBook;
            _getCommentsCountUseCase = getCommentsCountUseCase;

        }

        [HttpGet("{bookId:Guid}/{page:int}/{pageSize:int}")]
        public async Task<IActionResult> GetByBook([FromRoute] Guid bookId, int page, int pageSize)
        {
            var comments = await _getCommentsByBookUseCase.Execute(bookId, page, pageSize);
            var totalCount =  _getCommentsCountUseCase.Execute(bookId);
            var response = new PaginatedResponseDto<ResponseCommentDto>
            {
                Items = comments,
                TotalCount = totalCount
            };
            return Ok(response);
        }
    }
}
