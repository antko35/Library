using Library.Core.Abstractions.IInfrastructure;
using Library.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class ImageService : IImageService
    {
        //public ImageService() { }
        public async Task<string> UploadCover(BookEntity book, IFormFile file)
        {
            if (!string.IsNullOrEmpty(book.CoverImagePath))
            {
                var oldFilePath = Path.Combine("wwwroot", "uploads", book.CoverImagePath);

                // Удаляем старую фотографию, если она существует
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot", "uploads", fileName);

            // Сохранение файла на сервере
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
        public void DeleteCover(string? CoverImagePath)
        {
            if (!string.IsNullOrEmpty(CoverImagePath))
            {
                var oldFilePath = Path.Combine("wwwroot", "uploads", CoverImagePath);

                // Удаляем старую фотографию, если она существует
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                
            }
            
        }
    }
}
