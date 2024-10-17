using Library.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class GenreRepository : GenericRepository<GenreEntity>, IGenreRepository
    {
        public GenreRepository(LibraryDbContext context) : base(context) { }
 
        public async Task<GenreEntity?> IsExistByName(string name)
        {
            return await context.Genres
                .AsNoTracking()
                .FirstOrDefaultAsync(x=> x.Genre.ToLower() == name.ToLower());
                //.AnyAsync(x => x.Genre.ToLower() == name.ToLower());
        }
    }
}
