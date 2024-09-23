
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

       
    }
}
