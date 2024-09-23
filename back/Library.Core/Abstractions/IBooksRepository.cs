using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IBooksRepository
    {
        Task<BookEntity?> AlreadyExist(string ISBN);
        Task<Guid> Create(BookEntity bookEntity);
        Task<List<BookEntity>> GetAll();
        Task<BookEntity?> GetById(Guid id);
        Task<List<BookEntity>> GetByPage(int page, int pageSize);
    }
}