using Library.Application.Services;
using Library.Core.Contracts.Author;
using Library.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("Author")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseAuthorDto>>> GetAll()
        {
            var authors = await _authorService.GetAll();
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
        [HttpPost]
        public async Task<ActionResult<ResponseAuthorDto>> Create([FromBody] RequestAuthorDto requestAuthorDto) {
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
        
    }
}
