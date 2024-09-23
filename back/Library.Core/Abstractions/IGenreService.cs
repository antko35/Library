using Library.Core.Contracts.Genre;

namespace Library.Application.Services
{
    public interface IGenreService
    {
        Task<ResponseGenreDto> Create(RequestGenreDto requestGenreDto);
        Task<List<ResponseGenreDto>> GetAll();
    }
}