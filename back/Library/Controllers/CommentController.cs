using Library.Application.Contracts.Comment;
using Library.Application.Use_Cases.Comment;
using Library.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    //8e855e6e-818c-4a6d-973a-e02d0129bbec
    [ApiController]
    [Authorize]
    [Route("Comments")]
    public class CommentController : ControllerBase
    {
        private readonly GetCommentsByBookUseCase _getCommentsByBookUseCase;
        private readonly GetCommentsCountUseCase _getCommentsCountUseCase;
        private readonly CreateCommentUseCase _createComment;
        public CommentController(
            GetCommentsByBookUseCase getCommentsByBook,
            GetCommentsCountUseCase getCommentsCountUseCase,
            CreateCommentUseCase createCommentUseCase
            )
        {
            _getCommentsByBookUseCase = getCommentsByBook;
            _getCommentsCountUseCase = getCommentsCountUseCase;
            _createComment = createCommentUseCase;
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
        [HttpPost]
        public async Task<ActionResult> CreateComment([FromBody] RequestCreateCommentDto commentToCreate)
        {
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            await _createComment.Execute(commentToCreate, userId);
            return Ok();
        }
    }
}
