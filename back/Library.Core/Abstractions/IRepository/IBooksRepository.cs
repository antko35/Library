using Library.Core.Entities;

namespace Library.Core.Abstractions.IRepository
{
    public interface IBooksRepository : IGenericRepository<BookEntity>
    {
        Task BorrowBook(Guid id, Guid userId);
        Task<BookEntity?> GetByISBN(string isbn);
        Task<List<BookEntity>> GetByPage(int page, int pageSize, string search);
        Task<int> GetCount();
        Task ReturnBook(Guid id, Guid userId);
        Task UploadCover(Guid bookd, string fileName);
    }
}