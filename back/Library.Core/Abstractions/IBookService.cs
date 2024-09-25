using Library.Core.Contracts.Book;
using Library.Persistence.Entities;

namespace Library.Application.Servises
{
    public interface IBookService
    {
        Task BorrowBook(Guid bookId);
        Task<ResponseBookDto?> Create(RequestBookDto CreateBookDto);
        Task Delete(Guid id);
        Task<List<ResponseBookDto>> GetAll();
        Task<ResponseBookDto> GetById(Guid id);
        Task<ResponseBookDto> GetByIsbn(string isbn);
        Task<ResponseBookDto> Update(RequestBookDto requestBookDto);
    }
}