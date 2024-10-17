using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IBooksRepository : IGenericRepository<BookEntity>
    {
        Task BorrowBook(Guid id, Guid userId);
        Task<BookEntity?> GetByISBN(string isbn);
        Task<List<BookEntity>> GetByPage(int page, int pageSize);
        Task<int> GetCount();
        Task ReturnBook(Guid id);
        Task UploadCover(Guid bookd, string fileName);
    }
}