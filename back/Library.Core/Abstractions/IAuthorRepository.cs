using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IAuthorRepository
    {
        Task Create(AuthorEntity author);
        Task<List<AuthorEntity>> GetAll();
        Task<AuthorEntity?> GetById(Guid id);
        Task<bool> IsExist(Guid id);
        Task<bool> IsExist(string name, string surname, DateTime birthDate);
    }
}