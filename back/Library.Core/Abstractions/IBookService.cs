using Library.Core.Contracts.Book;
using Library.Persistence.Entities;

namespace Library.Application.Servises
{
    public interface IBookService
    {
        Task<ResponseBookDto?> Create(RequestBookDto CreateBookDto);
        Task<List<ResponseBookDto>> GetAll();
        Task<ResponseBookDto> GetById(Guid id);
        Task<ResponseBookDto> GetByIsbn(string isbn);
    }
}