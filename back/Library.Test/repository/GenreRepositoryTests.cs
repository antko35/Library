using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using Library.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Library.Test.repository
{
    public class GenreRepositoryTests
    {
        private readonly LibraryDbContext _context;
        private readonly GenreRepository _genreRepository;

        public GenreRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryTestDb")
                .Options;

            _context = new LibraryDbContext(options);
            _genreRepository = new GenreRepository(_context);
        }

        private void ClearDatabase()
        {
            _context.Genres.RemoveRange(_context.Genres);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfGenres()
        {
            ClearDatabase();
            var genre1 = new GenreEntity { Id = Guid.NewGuid(), Genre = "Fiction" };
            var genre2 = new GenreEntity { Id = Guid.NewGuid(), Genre = "Fantasy" };
            _context.Genres.AddRange(genre1, genre2);
            _context.SaveChanges();

            var result = await _genreRepository.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Fiction", result[0].Genre);
            Assert.Equal("Fantasy", result[1].Genre);
        }

        [Fact]
        public async Task Create_ShouldAddGenreToDatabase()
        {
            ClearDatabase();
            var genreEntity = new GenreEntity { Id = Guid.NewGuid(), Genre = "Fiction" };

            await _genreRepository.Create(genreEntity);

            var genresInDb = await _context.Genres.ToListAsync();
            Assert.Single(genresInDb); 
            Assert.Equal("Fiction", genresInDb[0].Genre); 
        }
    }
}
