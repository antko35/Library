using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using Library.Persistence.Entities;

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