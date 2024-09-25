
using Library.Application.Servises;
using Library.Persistence.Configurations;
using Microsoft.AspNetCore.Mvc;
using Library.Core.Contracts.Book;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("Books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookServise;
        public BookController(IBookService service)
        {
            _bookServise = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseBookDto>>> GetAll()
        {
            var books = await _bookServise.GetAll();
            return Ok(books);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
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
        [Route("{isbn}")]
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

        [HttpPost]
        public async Task<ActionResult<ResponseBookDto>> Create([FromBody] RequestBookDto createBookDto)
        {
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

        [HttpPut]
        public async Task<ActionResult<ResponseBookDto>> Update([FromBody] RequestBookDto requestBookDto)
        {
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

        [HttpPost("{bookId:Guid}")]
        public async Task<ActionResult> BorrowBook([FromRoute] Guid bookId)
        {
            try
            {
                await _bookServise.BorrowBook(bookId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        [HttpDelete("{id:Guid}")]
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
