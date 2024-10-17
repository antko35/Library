using FluentValidation;
using FluentValidation.Results;
using Library.Core.Abstractions.IService;
using Library.Core.Contracts.Author;
using Library.Core.Contracts.Book;
using Library.Persistence.Repositories;
using Library.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Author")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IValidator<RequestAuthorDto> _validator;
        private readonly IValidator<RequestUpdateAuthorDto> _updateValidator;
        public AuthorController(IAuthorService authorService, IValidator<RequestAuthorDto> validator, IValidator<RequestUpdateAuthorDto> updateValidator)
        {
            _authorService = authorService;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseAuthorDto>>> GetAll()
        {
            var authors = await _authorService.GetAll();
            //var auth = unitOfWork.AuthorRepository.Get();
            return Ok(authors);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<RequestAuthorDto>> GetById(Guid id)
        {
            try
            {
                var book = await _authorService.GetById(id); 
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
                var books = await _authorService.GetBooksByAuthor(authorId);
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
                var author = await _authorService.Create(requestAuthorDto); 
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
                var author = await _authorService.Update(requestUpdateAuthorDto);
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
                await _authorService.Delete(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
