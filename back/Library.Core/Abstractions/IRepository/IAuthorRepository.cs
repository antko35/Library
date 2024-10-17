using Library.Persistence.Entities;

namespace Library.Core.Abstractions.IRepository
{
    public interface IAuthorRepository : IGenericRepository<AuthorEntity>
    {
        Task<List<BookEntity>> GetBookByAuthor(Guid authorId);
        Task<bool> IsExist(Guid id);
        Task<bool> IsExist(string name, string surname, DateOnly birthDate);
    }
}