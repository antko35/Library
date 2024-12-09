using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        private IAuthorRepository authorRepository;
        private IBooksRepository bookRepository;
        private IGenreRepository genreRepository;
        private IUserRepository userRepository;
        private ICommentRepository commentRepository;


        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                this.authorRepository ??= new AuthorRepository(_context);
                return authorRepository;
            }
        }

        public IBooksRepository BookRepository
        {
            get
            {
                this.bookRepository ??= new BooksRepository(_context);
                return bookRepository;
            }
        }

        public IGenreRepository GenreRepository
        {
            get
            {
                this.genreRepository ??= new GenreRepository(_context);
                return genreRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                this.userRepository ??= new UserRepository(_context);
                return userRepository;
            }
        }

        public ICommentRepository CommentRepository
        {
            get 
            {
                commentRepository ??= new CommentRepository(_context);
                return commentRepository;
            }
        }
 

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
