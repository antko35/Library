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
    public class UnitOfWork : IDisposable
    {
        private LibraryDbContext context = new LibraryDbContext();

        private GenericRepository<AuthorEntity> authorRepository;
        private GenericRepository<BookEntity> bookRepository;
        private GenericRepository<GenreEntity> genreRepository;
        private GenericRepository<UserEntity> userRepository;

        public GenericRepository<AuthorEntity> AuthorRepository
        {
            get
            {
                if (this.authorRepository == null)
                {
                    this.authorRepository = new GenericRepository<AuthorEntity>(context);
                }
                return authorRepository;
            }
        }

        public GenericRepository<BookEntity> BookRepository
        {
            get
            {
                if (this.bookRepository == null)
                {
                    this.bookRepository = new GenericRepository<BookEntity>(context);
                }
                return bookRepository;
            }
        }

        public GenericRepository<GenreEntity> GenreRepository
        {
            get
            {
                if (this.genreRepository == null)
                {
                    this.genreRepository = new GenericRepository<GenreEntity>(context);
                }
                return genreRepository;
            }
        }

        public GenericRepository<UserEntity> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<UserEntity>(context);
                }
                return userRepository;
            }
        }


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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
