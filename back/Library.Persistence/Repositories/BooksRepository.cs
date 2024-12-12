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
      
        public async Task<List<BookEntity>> GetByPage(int page, int pageSize, string search)
        {
            var query = context.Books.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Title.ToLower().Contains(search));
            }
            return await query
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

            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id) ?? throw new ArgumentNullException();

            var user = await context.Users.Include(u => u.Books).FirstOrDefaultAsync(u => u.Id == userId);

            user.Books.Add(book);

        }

        public async Task ReturnBook(Guid id, Guid userId)
        {
            var book = await context.Books.FirstAsync(x => x.Id == id);
            var user = await context.Users.Include(x => x.Books).FirstAsync(x => x.Id == userId);

            user.Books.Remove(book);
           
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
