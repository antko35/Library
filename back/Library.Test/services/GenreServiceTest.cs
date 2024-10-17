using Moq;
using Library.Application.Services;
using Library.Core.Contracts.Genre;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using Library.Persistence;
using Library.Core.Abstractions.IRepository;

namespace Library.Test.services
{
    public class GenreServiceTests
    {
        private readonly GenreService _genreService;
        private readonly GenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly LibraryDbContext _context;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;

        public GenreServiceTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryTestDb")
                .Options;

            _context = new LibraryDbContext(options);

            _genreRepositoryMock = new Mock<IGenreRepository>();

            _genreRepository = new GenreRepository(_context);
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GenreEntity, ResponseGenreDto>().ReverseMap();
                cfg.CreateMap<RequestGenreDto, GenreEntity>();
            }).CreateMapper();

            _genreService = new GenreService(_genreRepository, _mapper);
        }

        private void ClearDatabase()
        {
            _context.Genres.RemoveRange(_context.Genres);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Delete_WhenGenreExists_ShouldDeleteGenre()
        {
            // Arrange
            var existingGenreId = Guid.NewGuid();

            _genreRepositoryMock.Setup(repo => repo.Delete(existingGenreId))
                .ReturnsAsync(1);

            // Act
            var genreServiceWithMockRepo = new GenreService(_genreRepositoryMock.Object, _mapper);
            await genreServiceWithMockRepo.Delete(existingGenreId);

            // Assert
            _genreRepositoryMock.Verify(repo => repo.Delete(existingGenreId), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenGenreDoesNotExist_ShouldThrowException()
        {
            var nonExistentGenreId = Guid.NewGuid();

            _genreRepositoryMock.Setup(repo => repo.Delete(nonExistentGenreId))
                .ReturnsAsync(0);

            var genreServiceWithMockRepo = new GenreService(_genreRepositoryMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<Exception>(() => genreServiceWithMockRepo.Delete(nonExistentGenreId));

            Assert.Equal("Genre doesnt exist", ex.Message);
        }


        [Fact]
        public async Task GetAll_ShouldReturnListOfGenres()
        {
            ClearDatabase();
            var genre1 = new GenreEntity { Id = Guid.NewGuid(), Genre = "Fiction" };
            var genre2 = new GenreEntity { Id = Guid.NewGuid(), Genre = "Fantasy" };
            _context.Genres.AddRange(genre1, genre2);
            _context.SaveChanges();

            var result = await _genreService.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Fiction", result[0].Genre);
            Assert.Equal("Fantasy", result[1].Genre);
        }

        [Fact]
        public async Task Create_ShouldAddGenreToDatabase()
        {
            ClearDatabase();
            var requestGenreDto = new RequestGenreDto { Genre = "Fiction" };

            var result = await _genreService.Create(requestGenreDto);

            var genresInDb = await _context.Genres.ToListAsync();
            Assert.Single(genresInDb);
            Assert.Equal("Fiction", genresInDb[0].Genre);
            Assert.Equal("Fiction", result.Genre);
        }

        [Fact]
        public async Task Create_WhenGenreAlreadyExists_ShouldThrowException()
        {
            ClearDatabase();
            var genreEntity = new GenreEntity { Id = Guid.NewGuid(), Genre = "Fiction" };
            _context.Genres.Add(genreEntity);
            _context.SaveChanges();

            var requestGenreDto = new RequestGenreDto { Genre = "Fiction" };

            var ex = await Assert.ThrowsAsync<Exception>(() => _genreService.Create(requestGenreDto));
            Assert.Equal("Genre already exist", ex.Message);
        }
    }
}
