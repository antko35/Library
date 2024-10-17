using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IGenreRepository : IGenericRepository<GenreEntity>
    {
        Task<GenreEntity?> IsExistByName(string name);
    }
}