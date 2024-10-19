using Library.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Abstractions.IInfrastructure
{
    public interface IImageService
    {
        Task<string> UploadCover(BookEntity book, IFormFile file);
    }
}