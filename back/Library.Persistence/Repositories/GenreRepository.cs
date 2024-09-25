using Library.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly LibraryDbContext _context;
        public GenreRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsExist(Guid id)
        {
            return await _context.Genres
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
        public async Task<bool> IsExistByName(string name)
        {
            return await _context.Genres
                .AsNoTracking()
                .AnyAsync(x => x.Genre.ToLower() == name.ToLower());
        }
        public async Task<List<GenreEntity>> GetAll()
        {
            return await _context.Genres
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task Create(GenreEntity genreEntity)
        {
            await _context.Genres.AddAsync(genreEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(Guid id)
        {
            var count = await _context.Genres
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            return count;
        }
    }
}
