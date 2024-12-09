using Library.Core.Entities;

namespace Library.Core.Abstractions.IRepository
{
    public interface ICommentRepository
    {
        Task<List<CommentEntity>> GetByBook(Guid bookId, int page, int pageSize);
        int GetCount(Guid bookId);
    }
}