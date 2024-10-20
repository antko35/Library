using FluentValidation;
using FluentValidation.Results;
using Library.Application.Contracts.Author;
using Library.Application.Contracts.Book;
using Library.Application.Use_Cases.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Author")]
    public class AuthorController : ControllerBase
    {
        private readonly GetAllAuthorsUseCase _getAllAuthorsUseCase;
        private readonly GetAuthorByIdUseCase _getAuthorByIdUseCase;
        private readonly GetBooksByAuthorUseCase _getBooksByAuthorUseCase;
        private readonly CreateAuthorUseCase _createAuthorUseCase;
        private readonly UpdateAuthorUseCase _updateAuthorUseCase;
        private readonly DeleteAuthorUseCase _deleteAuthorUseCase;

        private readonly IValidator<RequestAuthorDto> _validator;
        private readonly IValidator<RequestUpdateAuthorDto> _updateValidator;
        public AuthorController(
            IValidator<RequestAuthorDto> validator,
            IValidator<RequestUpdateAuthorDto> updateValidator,
            GetAuthorByIdUseCase getAuthorByIdUseCase,
            GetAllAuthorsUseCase getAllAuthorsUseCase,
            GetBooksByAuthorUseCase getBooksByAuthorUseCase,
            CreateAuthorUseCase createAuthorUseCase,
            UpdateAuthorUseCase updateAuthorUseCase,
            DeleteAuthorUseCase deleteAuthorUseCase
        )
        {
            _validator = validator;
            _updateValidator = updateValidator;
            _getAuthorByIdUseCase = getAuthorByIdUseCase;
            _getAllAuthorsUseCase = getAllAuthorsUseCase;
            _getBooksByAuthorUseCase = getBooksByAuthorUseCase;
            _createAuthorUseCase = createAuthorUseCase;
            _updateAuthorUseCase = updateAuthorUseCase;
            _deleteAuthorUseCase = deleteAuthorUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseAuthorDto>>> GetAll()
        {
            var authors = await _getAllAuthorsUseCase.Execute();
            return Ok(authors);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<RequestAuthorDto>> GetById(Guid id)
        {
            var book = await _getAuthorByIdUseCase.Execute(id);
            return Ok(book);
        }

        [HttpGet("/getBooks/{authorId:Guid}")]
        public async Task<ActionResult<List<ResponseBookDto>>> GetBooksByAuthor(Guid authorId)
        {
            var books = await _getBooksByAuthorUseCase.Execute(authorId);
            return Ok(books);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ResponseAuthorDto>> Create([FromBody] RequestAuthorDto requestAuthorDto)
        {
            var author = await _createAuthorUseCase.Execute(requestAuthorDto);
            return Ok(author);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ResponseAuthorDto>> Update([FromBody] RequestUpdateAuthorDto requestUpdateAuthorDto)
        {
            var author = await _updateAuthorUseCase.Execete(requestUpdateAuthorDto);
            return Ok(author);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{Id:Guid}")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            await _deleteAuthorUseCase.Execute(Id);
            return Ok();
        }
    }
}
