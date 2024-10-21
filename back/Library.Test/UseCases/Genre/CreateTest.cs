using AutoMapper;
using Library.Application.DTOs.Genre;
using Library.Application.Use_Cases.Genre;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using Library.Persistence.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Library.Test.UseCases.Genre
{
    public class CreateGenreUseCaseTests
    {
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly CreateGenreUseCase _createGenreUseCase;

        public CreateGenreUseCaseTests()
        {
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IValidationService>();

            _unitOfWorkMock.Setup(uow => uow.GenreRepository).Returns(_genreRepositoryMock.Object);

            _createGenreUseCase = new CreateGenreUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task Create_Successfully()
        {
            // Arrange
            var requestGenreDto = new RequestGenreDto { Genre = "Fiction" };
            var genreEntity = new GenreEntity { Id = Guid.NewGuid(), Genre = requestGenreDto.Genre };
            var responseGenreDto = new ResponseGenreDto { Id = genreEntity.Id, Genre = genreEntity.Genre };

            _validationServiceMock.Setup(v => v.ValidateAsync(requestGenreDto)).Returns(Task.CompletedTask);

            _genreRepositoryMock.Setup(repo => repo.IsExistByName(requestGenreDto.Genre)).ReturnsAsync((GenreEntity)null);

            _mapperMock.Setup(mapper => mapper.Map<GenreEntity>(requestGenreDto)).Returns(genreEntity);
            _genreRepositoryMock.Setup(repo => repo.Insert(genreEntity)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.Save()).Returns(Task.CompletedTask);
            _genreRepositoryMock.Setup(repo => repo.GetByID(genreEntity.Id)).ReturnsAsync(genreEntity);
            _mapperMock.Setup(mapper => mapper.Map<ResponseGenreDto>(genreEntity)).Returns(responseGenreDto);

            // Act
            var result = await _createGenreUseCase.Execute(requestGenreDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseGenreDto.Id, result.Id);
            Assert.Equal(responseGenreDto.Genre, result.Genre);

            _genreRepositoryMock.Verify(repo => repo.Insert(It.IsAny<GenreEntity>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_AlreadyExists()
        {
            // Arrange
            var requestGenreDto = new RequestGenreDto { Genre = "Fiction" };
            var existingGenre = new GenreEntity { Id = Guid.NewGuid(), Genre = requestGenreDto.Genre };

            _validationServiceMock.Setup(v => v.ValidateAsync(requestGenreDto)).Returns(Task.CompletedTask);
            _genreRepositoryMock.Setup(repo => repo.IsExistByName(requestGenreDto.Genre)).ReturnsAsync(existingGenre);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _createGenreUseCase.Execute(requestGenreDto));
            Assert.Equal("Genre already exists", exception.Message);

            _genreRepositoryMock.Verify(repo => repo.Insert(It.IsAny<GenreEntity>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Never);
        }
    }
}
