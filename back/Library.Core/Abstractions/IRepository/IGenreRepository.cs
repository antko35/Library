using Library.Persistence.Entities;

namespace Library.Core.Abstractions.IRepository
{
    public interface IGenreRepository : IGenericRepository<GenreEntity>
    {
        Task<GenreEntity?> IsExistByName(string name);
    }
}