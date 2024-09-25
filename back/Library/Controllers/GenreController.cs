using Library.Application.Services;
using Library.Core.Contracts.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("Genre")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService; 
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ResponseGenreDto>>> GetAll()
        {
            var genres = await _genreService.GetAll();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<ActionResult<ResponseGenreDto>> Create([FromBody] RequestGenreDto requestGenreDto)
        {
            try
            {
                var genre = await _genreService.Create(requestGenreDto);
                return Ok(genre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _genreService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
