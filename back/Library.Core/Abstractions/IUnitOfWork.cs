using Library.Core.Entities;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;

namespace Library.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IBooksRepository BookRepository { get; }
        IGenreRepository GenreRepository { get; }
        IUserRepository UserRepository { get; }

        void Dispose();
        Task Save();
    }
}