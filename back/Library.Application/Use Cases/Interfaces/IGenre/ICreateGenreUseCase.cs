using Library.Application.DTOs.Genre;

namespace Library.Application.Use_Cases.Genre
{
    public interface ICreateGenreUseCase
    {
        Task<ResponseGenreDto> Execute(RequestGenreDto requestGenreDto);
    }
}