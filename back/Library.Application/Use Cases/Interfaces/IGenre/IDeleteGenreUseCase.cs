namespace Library.Application.Use_Cases.Genre
{
    public interface IDeleteGenreUseCase
    {
        Task Delete(Guid Id);
    }
}