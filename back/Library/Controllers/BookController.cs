using Library.Persistence.Configurations;
using Microsoft.AspNetCore.Mvc;
using Library.Core.Contracts.Book;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using FluentValidation;
using Library.Core.Contracts.Author;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Library.Core.Abstractions.IService;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookServise;
        private readonly IValidator<RequestBookDto> _validator;
        public BookController(IBookService service, IValidator<RequestBookDto> validator)
        {
            _bookServise = service;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseBookDto>>> GetAll()
        {
            var books = await _bookServise.GetAll();
            return Ok(books);
        }

        [HttpGet]
        [Route("getBypage/{page:int}/{pageSize:int}")]
        public async Task<ActionResult<List<ResponseBookDto>>> GetPage([FromRoute] int page, int pageSize)
        {
            var books = await _bookServise.GetByPage(page, pageSize);
            return Ok(books);
        }

        [HttpGet]
        [Route("getById/{Id:Guid}")]
        public async Task<ActionResult<ResponseBookDto>> GetById([FromRoute] Guid Id)
        {
            try
            {
                var book = await _bookServise.GetById(Id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("getByIsbn/{isbn}")]
        public async Task<ActionResult<ResponseBookDto>> GetByIsbn([FromRoute] string isbn)
        {
            try
            {
                var book = await _bookServise.GetByIsbn(isbn);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<int>> GetBooksCount()
        {
            var cont = await _bookServise.GetBooksCount();
            return Ok(cont);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<ResponseBookDto>> Create([FromBody] RequestBookDto createBookDto)
        {
            ValidationResult result = await _validator.ValidateAsync(createBookDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var createdBook = await _bookServise.Create(createBookDto);
                return Ok(createdBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("update")]
        public async Task<ActionResult<ResponseBookDto>> Update([FromBody] RequestUpdateBookDto requestBookDto)
        {
            /*ValidationResult result = await _validator.ValidateAsync(requestBookDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }*/

            try
            {
                var book = await _bookServise.Update(requestBookDto);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("borrow/{bookId:Guid}")]
        public async Task<ActionResult> BorrowBook([FromRoute] Guid bookId)
        {
            try
            {
                var userId = User.Claims.First(x => x.Type == "UserId").Value;
                Guid UserId = Guid.Parse(userId);
                await _bookServise.BorrowBook(bookId, UserId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("return/{bookId:Guid}")]
        public async Task<ActionResult> ReturnBook([FromRoute] Guid bookId)
        {
            try
            {
                var userId = User.Claims.First(x => x.Type == "UserId").Value;
                Guid UserId = Guid.Parse(userId);

                await _bookServise.ReturnBook(bookId, UserId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("upload-cover/{bookId}")]
        public async Task<ActionResult> UploadCover(Guid bookId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Invalid file.");

                await _bookServise.UploadCover(bookId, file);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Authorize(Policy = "Admin")]
        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _bookServise.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
