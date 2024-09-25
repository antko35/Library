using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IAuthorRepository
    {
        Task Create(AuthorEntity author);
        Task<int> Delete(Guid id);
        Task<List<AuthorEntity>> GetAll();
        Task<List<BookEntity>> GetBookByAuthor(Guid authorId);
        Task<AuthorEntity?> GetById(Guid id);
        Task<bool> IsExist(Guid id);
        Task<bool> IsExist(string name, string surname, DateOnly birthDate);
        Task Update(AuthorEntity forUpdate);
    }
}