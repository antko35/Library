using Library.Persistence;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Library.Test.Repositories
{
    public class GenreRepositoryTests
    {
        private readonly LibraryDbContext _context;
        private readonly GenreRepository _genreRepository;

        public GenreRepositoryTests()
        {
            // Настройка InMemory базы данных
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryDatabase")
                .Options;

            _context = new LibraryDbContext(options);
            _genreRepository = new GenreRepository(_context);
        }

        [Fact]
        public async Task IsExistByName_Successfully()
        {
            // Arrange
            var genreName = "Fiction";
            var genreEntity = new GenreEntity { Id = Guid.NewGuid(), Genre = genreName };
            await _context.Genres.AddAsync(genreEntity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _genreRepository.IsExistByName(genreName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(genreName, result.Genre);
        }

        [Fact]
        public async Task IsExistByName_ShouldReturnNull_WhenGenreDoesNotExist()
        {
            // Arrange
            var genreName = "Non-Existent Genre";

            // Act
            var result = await _genreRepository.IsExistByName(genreName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Insert_Successfully()
        {
            // Arrange
            var genreEntity = new GenreEntity { Id = Guid.NewGuid(), Genre = "Science Fiction" };

            // Act
            await _genreRepository.Insert(genreEntity);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _genreRepository.GetByID(genreEntity.Id);
            Assert.NotNull(result);
            Assert.Equal(genreEntity.Genre, result.Genre);
        }
    }
}
