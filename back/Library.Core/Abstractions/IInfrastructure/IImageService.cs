using Library.Persistence.Entities;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Abstractions
{
    public interface IImageService
    {
        Task<string> UploadCover(BookEntity book, IFormFile file);
    }
}