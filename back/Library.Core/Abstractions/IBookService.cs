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
        Task<int> GetBooksCount();
        Task<ResponseBookDto> GetById(Guid id);
        Task<ResponseBookDto> GetByIsbn(string isbn);
        Task<List<ResponseBookDto>> GetByPage(int page, int pageSize);
        Task ReturnBook(Guid bookId, Guid userId);
        Task<ResponseBookDto> Update(RequestUpdateBookDto requestBookDto);
        Task UploadCover(Guid bookId, IFormFile file);
    }
}