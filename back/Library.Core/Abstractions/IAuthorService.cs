

using Library.Core.Contracts.Author;
using Library.Core.Contracts.Book;

namespace Library.Application.Services
{
    public interface IAuthorService
    {
        Task<ResponseAuthorDto> Create(RequestAuthorDto requestAuthorDto);
        Task Delete(Guid Id);
        Task<List<ResponseAuthorDto>> GetAll();
        Task<List<ResponseBookDto>> GetBooksByAuthor(Guid authorId);
        Task<ResponseAuthorDto> GetById(Guid id);
        Task<ResponseAuthorDto> Update(RequestUpdateAuthorDto requestUpdateAuthorDto);
    }
}