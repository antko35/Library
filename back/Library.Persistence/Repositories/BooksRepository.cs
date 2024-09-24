using Library.Core.Contracts;
using Library.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly LibraryDbContext _context;
        public BooksRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookEntity>> GetAll()
        {
            return await _context.Books
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BookEntity?> GetById(Guid id)
        {
            return await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<BookEntity>> GetByPage(int page, int pageSize)
        {
            return await _context.Books
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<BookEntity?> AlreadyExist(string ISBN)
        {
            return await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ISBN.Equals(ISBN));
        }

        public async Task<Guid> Create(BookEntity bookEntity)
        {
            await _context.Books.AddAsync(bookEntity);
            await _context.SaveChangesAsync();
            return bookEntity.Id;
        }

        public async Task Update(Guid existingId, BookEntity forUpdate)
        {
            await _context.Books
                .Where(x => x.Id == existingId)
                .ExecuteUpdateAsync(setters =>setters
                    .SetProperty(b => b.GenreId, forUpdate.GenreId)
                    .SetProperty(b => b.AuthorId, forUpdate.AuthorId)
                    .SetProperty(b => b.Description, forUpdate.Description)
                    .SetProperty(b => b.Title, forUpdate.Title)
                );
        }
        public async Task<int> Delete(Guid id)
        {
            var count = await _context.Books
                .Where (x => x.Id == id)
                .ExecuteDeleteAsync();
            return count;
        }
    }
}
