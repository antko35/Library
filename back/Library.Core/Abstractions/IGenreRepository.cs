using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IGenreRepository
    {
        Task Create(GenreEntity genreEntity);
        Task<int> Delete(Guid id);
        Task<List<GenreEntity>> GetAll();
        Task<bool> IsExist(Guid id);
        Task<bool> IsExistByName(string name);
    }
}