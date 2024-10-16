using Library.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class AuthorRepository : GenericRepository<AuthorEntity>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context) : base(context)
        { }
        
       
        public async Task<bool> IsExist(Guid id)
        {
            return await context.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
        public async Task<bool> IsExist(string name, string surname, DateOnly birthDate)
        {
            return await context.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Name == name && x.Surname == surname && x.BirthDate == birthDate);
        }

        public async Task<List<BookEntity>> GetBookByAuthor(Guid authorId)
        {
            var books = await context.Books
                .AsNoTracking()
                .Where(x => x.AuthorId == authorId)
                .ToListAsync();

            return books;
        }
    }
}
