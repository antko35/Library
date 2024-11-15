using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class BooksRepository : GenericRepository<BookEntity>, IBooksRepository
    {
        public BooksRepository(LibraryDbContext context) : base(context) { }
      
        public async Task<List<BookEntity>> GetByPage(int page, int pageSize)
        {
            /*IQueryable<BookEntity> query = context.Books.AsNoTracking();
            if (authorId.HasValue)
            {
                query.Where(a => a.Id == authorId);
            }
            if (genreId.HasValue)
            {
                query.Where(g => g. == genreId);
            }*/
            return await context.Books
                .AsNoTracking()
                .OrderBy(x => x.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> GetCount()
        {
            return await context.Books.CountAsync();
        }
        public async Task<BookEntity?> GetByISBN(string isbn)
        {
            var book = await context.Books.FirstOrDefaultAsync(x => x.ISBN == isbn);
            return book;
        }
        public async Task BorrowBook(Guid id, Guid userId)
        {
            await context.Books
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.UserId, userId)
                .SetProperty(b => b.BorrowDate, DateOnly.FromDateTime(DateTime.Now))
                .SetProperty(b => b.ReturnDate, DateOnly.FromDateTime(DateTime.Now.AddDays(7)))
                );
        }

        public async Task ReturnBook(Guid id)
        {
            await context.Books
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.UserId, (Guid?)null)
                .SetProperty(b => b.BorrowDate, (DateOnly?)null)
                .SetProperty(b => b.ReturnDate, (DateOnly?)null)
                );
        }

        public async Task UploadCover(Guid bookd, string fileName)
        {
            await context.Books
                .Where(x => x.Id == bookd)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(b => b.CoverImagePath, fileName));
        }
    }
}
