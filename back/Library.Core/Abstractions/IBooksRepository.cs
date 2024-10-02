using Library.Persistence.Entities;

namespace Library.Persistence.Repositories
{
    public interface IBooksRepository
    {
        Task<BookEntity?> AlreadyExist(string ISBN);
        Task BorrowBook(Guid id, Guid userId);
        Task<Guid> Create(BookEntity bookEntity);
        Task<int> Delete(Guid id);
        Task<List<BookEntity>> GetAll();
        Task<BookEntity?> GetById(Guid id);
        Task<List<BookEntity>> GetByPage(int page, int pageSize);
        Task<int> GetCount();
        Task ReturnBook(Guid id);
        Task Update(Guid existingId, BookEntity forUpdate);
        Task UploadCover(Guid bookd, string fileName);
    }
}