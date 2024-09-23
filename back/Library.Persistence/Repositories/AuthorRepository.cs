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
        public async Task<bool> IsExist(Guid id)
        {
            return await _context.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
        public async Task Create(AuthorEntity author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }
        public async Task<List<AuthorEntity>> GetAll()
        {
            return await _context.Authors
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> IsExist(string name, string surname, DateTime birthDate)
        {
            return await _context.Authors
                .AsNoTracking()
                .AnyAsync(x => x.Name == name && x.Surname == surname && x.BirthDate == birthDate);
        }
    }
}
