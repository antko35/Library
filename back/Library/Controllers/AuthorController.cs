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
            try
            {
                var book = await _getAuthorByIdUseCase.Execute(id); 
                return Ok(book);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("/getBooks/{authorId:Guid}")]
        public async Task<ActionResult<List<ResponseBookDto>>> GetBooksByAuthor(Guid authorId)
        {
            try
            {
                var books = await _getBooksByAuthorUseCase.Execute(authorId);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ResponseAuthorDto>> Create([FromBody] RequestAuthorDto requestAuthorDto) 
        {
            ValidationResult result = await _validator.ValidateAsync(requestAuthorDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var author = await _createAuthorUseCase.Execute(requestAuthorDto);
                //var author = await _authorService.Create(requestAuthorDto); 
                return Ok(author);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ResponseAuthorDto>> Update([FromBody] RequestUpdateAuthorDto requestUpdateAuthorDto)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(requestUpdateAuthorDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            try
            {
                var author = await _updateAuthorUseCase.Execete(requestUpdateAuthorDto);
                return Ok(author);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{Id:Guid}")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            try
            {
                await _deleteAuthorUseCase.Execute(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
