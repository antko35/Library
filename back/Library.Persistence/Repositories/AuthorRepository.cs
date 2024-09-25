using Library.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;
        public AuthorRepository(LibraryDbContext context)
        {

            _context = context;

        }
        
        public async Task<List<AuthorEntity>> GetAll()
        {
            return await _context.Authors
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> IsExist(Guid id)
        {
            return await _context.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
        public async Task<bool> IsExist(string name, string surname, DateOnly birthDate)
        {
            return await _context.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Name == name && x.Surname == surname && x.BirthDate == birthDate);
        }
        public async Task<AuthorEntity?> GetById(Guid id)
        {
            return await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BookEntity>> GetBookByAuthor(Guid authorId)
        {
            var books = await _context.Books
                .AsNoTracking()
                .Where(x => x.AuthorId == authorId)
                .ToListAsync();

            return books;
        }

        public async Task Create(AuthorEntity author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }
        
        public async Task Update(AuthorEntity forUpdate)
        {
            await _context.Authors
                .Where(a => a.Id == forUpdate.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.Name, forUpdate.Name)
                    .SetProperty(a => a.Surname, forUpdate.Surname)
                    .SetProperty(a => a.BirthDate, forUpdate.BirthDate)
                    .SetProperty(a => a.Country, forUpdate.Country)
                    );
        }

        public async Task<int> Delete(Guid id)
        {
            var count = await _context.Authors
                .Where (a => a.Id == id)
                .ExecuteDeleteAsync();
            return count;
        }
        
    }
}
