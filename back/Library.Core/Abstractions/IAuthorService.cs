using Library.Core.Contracts.Author;

namespace Library.Application.Services
{
    public interface IAuthorService
    {
        Task<ResponseAuthorDto> Create(RequestAuthorDto requestAuthorDto);
        Task Delete(Guid Id);
        Task<List<ResponseAuthorDto>> GetAll();
        Task<ResponseAuthorDto> GetById(Guid id);
        Task<ResponseAuthorDto> Update(RequestUpdateAuthorDto requestUpdateAuthorDto);
    }
}