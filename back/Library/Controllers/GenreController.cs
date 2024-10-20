using FluentValidation;
using FluentValidation.Results;
using Library.Application.DTOs.Genre;
using Library.Application.Use_Cases.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Genre")]
    public class GenreController : ControllerBase
    {
        private readonly ICreateGenreUseCase _createGenreUseCase;
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly IDeleteGenreUseCase _deleteGenreUseCase;

        private readonly IValidator<RequestGenreDto> _validator;
        public GenreController(
            IValidator<RequestGenreDto> validator,
            ICreateGenreUseCase createGenreUseCase,
            IGetAllUseCase getAllUseCase,
            IDeleteGenreUseCase deleteGenreUseCase
        )
        {
            _validator = validator;
            _createGenreUseCase = createGenreUseCase;
            _getAllUseCase = getAllUseCase;
            _deleteGenreUseCase = deleteGenreUseCase;
        }
        [HttpGet]
        public async Task<ActionResult<List<ResponseGenreDto>>> GetAll()
        {
            var genres = await _getAllUseCase.Execute();
            return Ok(genres);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ResponseGenreDto>> Create([FromBody] RequestGenreDto requestGenreDto)
        {
                var genre = await _createGenreUseCase.Execute(requestGenreDto);
                return Ok(genre);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
                await _deleteGenreUseCase.Delete(id);
                return Ok();
        }
    }
}
