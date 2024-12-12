using Library.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Abstractions.IInfrastructure
{
    public interface IImageService
    {
        void DeleteCover(string? CoverImagePath);
        Task<string> UploadCover(BookEntity book, IFormFile file);
    }
}