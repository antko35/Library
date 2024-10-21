using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Application.DTOs.Genre;
using Library.Application.Use_Cases.Genre;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using AutoMapper;
using Library.Persistence.Entities;
using System.Linq.Expressions;

namespace Library.Tests.UseCases.Genre
{
    public class GetAllUseCaseTests
    {
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllUseCase _getAllUseCase;

        public GetAllUseCaseTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _mapperMock = new Mock<IMapper>();

            // Setup unit of work to return the mocked genre repository
            _unitOfWorkMock.Setup(u => u.GenreRepository)
                           .Returns(_genreRepositoryMock.Object);

            // Initialize the use case with the mocked dependencies
            _getAllUseCase = new GetAllUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Get_All_Ok()
        {
            // Arrange
            var genreEntities = new List<GenreEntity>
            {
                new GenreEntity { Id = Guid.NewGuid(), Genre = "Fiction" },
                new GenreEntity { Id = Guid.NewGuid(), Genre = "Non-fiction" }
            };

            _genreRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<GenreEntity, bool>>>(),
                                             It.IsAny<Func<IQueryable<GenreEntity>, IOrderedQueryable<GenreEntity>>>(),
                                             It.IsAny<string>()))
                                .ReturnsAsync(genreEntities);

            var genreDtos = genreEntities.Select(genre => new ResponseGenreDto
            {
                Id = genre.Id,
                Genre = genre.Genre
            }).ToList();

            // Set up the mapper mock to map the genre entities to DTOs
            _mapperMock.Setup(mapper => mapper.Map<ResponseGenreDto>(It.IsAny<GenreEntity>()))
                       .Returns((GenreEntity genre) => new ResponseGenreDto
                       {
                           Id = genre.Id,
                           Genre = genre.Genre
                       });

            // Act
            var result = await _getAllUseCase.Execute();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(genreDtos.Count, result.Count);
            for (int i = 0; i < genreDtos.Count; i++)
            {
                Assert.Equal(genreDtos[i].Id, result[i].Id);
                Assert.Equal(genreDtos[i].Genre, result[i].Genre);
            }

            _genreRepositoryMock.Verify(repo => repo.Get(It.IsAny<Expression<Func<GenreEntity, bool>>>(),
                                             It.IsAny<Func<IQueryable<GenreEntity>, IOrderedQueryable<GenreEntity>>>(),
                                             It.IsAny<string>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ResponseGenreDto>(It.IsAny<GenreEntity>()), Times.Exactly(genreEntities.Count));
        }

        [Fact]
        public async Task Get_All_No_Genres()
        {
            // Arrange
            _genreRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<GenreEntity, bool>>>(),
                                             It.IsAny<Func<IQueryable<GenreEntity>, IOrderedQueryable<GenreEntity>>>(),
                                             It.IsAny<string>()))
                                .ReturnsAsync(new List<GenreEntity>());

            // Act
            var result = await _getAllUseCase.Execute();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _genreRepositoryMock.Verify(repo => repo.Get(It.IsAny<Expression<Func<GenreEntity, bool>>>(),
                                             It.IsAny<Func<IQueryable<GenreEntity>, IOrderedQueryable<GenreEntity>>>(),
                                             It.IsAny<string>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ResponseGenreDto>(It.IsAny<GenreEntity>()), Times.Never);
        }
    }
}
