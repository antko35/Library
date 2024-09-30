using Library.Core.Contracts.Book;
using Library.Persistence.Entities;
using Microsoft.AspNetCore.Http;

namespace Library.Application.Servises
{
    public interface IBookService
    {
        Task BorrowBook(Guid bookId, Guid userId);
        Task<ResponseBookDto?> Create(RequestBookDto CreateBookDto);
        Task Delete(Guid id);
        Task<List<ResponseBookDto>> GetAll();
        Task<ResponseBookDto> GetById(Guid id);
        Task<ResponseBookDto> GetByIsbn(string isbn);
        Task ReturnBook(Guid bookId, Guid userId);
        Task<ResponseBookDto> Update(RequestBookDto requestBookDto);
        Task UploadCover(Guid bookId, IFormFile file);
    }
}