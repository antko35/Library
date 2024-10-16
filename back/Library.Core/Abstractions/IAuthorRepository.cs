using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IAuthorRepository : IGenericRepository<AuthorEntity>
    {
        Task<List<BookEntity>> GetBookByAuthor(Guid authorId);
        Task<bool> IsExist(Guid id);
        Task<bool> IsExist(string name, string surname, DateOnly birthDate);
    }
}