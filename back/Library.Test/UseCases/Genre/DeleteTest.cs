using Library.Application.Use_Cases.Genre;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using Library.Persistence.Entities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Library.Test.UseCases.Genre
{
    public class DeleteGenreUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteGenreUseCase _deleteGenreUseCase;

        public DeleteGenreUseCaseTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _deleteGenreUseCase = new DeleteGenreUseCase(_unitOfWorkMock.Object, null);
        }

        [Fact]
        public async Task Delete_Successfully()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var existingGenre = new GenreEntity { Id = genreId, Genre = "Fiction" };

            _unitOfWorkMock.Setup(repo => repo.GenreRepository.GetByID(genreId)).ReturnsAsync(existingGenre);
            _unitOfWorkMock.Setup(repo => repo.GenreRepository.Delete(genreId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);

            // Act
            await _deleteGenreUseCase.Delete(genreId);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.GenreRepository.Delete(genreId), Times.Once);
            _unitOfWorkMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_GenreDoesNotExist()
        {
            // Arrange
            var genreId = Guid.NewGuid();

            _unitOfWorkMock.Setup(repo => repo.GenreRepository.GetByID(genreId)).ReturnsAsync((GenreEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _deleteGenreUseCase.Delete(genreId));
            Assert.Equal("Genre doesnt exist", exception.Message);

            _unitOfWorkMock.Verify(repo => repo.GenreRepository.Delete(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(repo => repo.Save(), Times.Never);
        }
    }
}
