using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using Library.Persistence.Entities;

namespace Library.Core.Abstractions
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IBooksRepository BookRepository { get; }
        IGenreRepository GenreRepository { get; }
        IUserRepository UserRepository { get; }
        ICommentRepository CommentRepository { get; }

        void Dispose();
        Task Save();
    }
}