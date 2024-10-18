using Library.Application.DTOs.Genre;

namespace Library.Application.Use_Cases.Genre
{
    public interface IGetAllUseCase
    {
        Task<List<ResponseGenreDto>> Execute();
    }
}