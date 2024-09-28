using FluentValidation;
using FluentValidation.Results;
using Library.Application.Services;
using Library.Core.Contracts.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Genre")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly IValidator<RequestGenreDto> _validator;
        public GenreController(IGenreService genreService, IValidator<RequestGenreDto> validator)
        {
            _genreService = genreService;
            _validator = validator;
        }
        [HttpGet]
        public async Task<ActionResult<List<ResponseGenreDto>>> GetAll()
        {
            var genres = await _genreService.GetAll();
            return Ok(genres);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ResponseGenreDto>> Create([FromBody] RequestGenreDto requestGenreDto)
        {
            ValidationResult result = await _validator.ValidateAsync(requestGenreDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

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

        [Authorize(Policy = "Admin")]
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
