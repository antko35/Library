using Library.Core.Contracts.Author;

namespace Library.Application.Services
{
    public interface IAuthorService
    {
        Task<ResponseAuthorDto> Create(RequestAuthorDto requestAuthorDto);
        Task<List<ResponseAuthorDto>> GetAll();
    }
}