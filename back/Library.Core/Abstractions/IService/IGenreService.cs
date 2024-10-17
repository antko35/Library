using Library.Core.Contracts.Genre;

namespace Library.Core.Abstractions.IService
{
    public interface IGenreService
    {
        Task<ResponseGenreDto> Create(RequestGenreDto requestGenreDto);
        Task Delete(Guid Id);
        Task<List<ResponseGenreDto>> GetAll();
    }
}