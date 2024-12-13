using Library.Core.Abstractions;
using Library.Core.Abstractions.IInfrastructure;
using Library.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.ExportToExcel
{
    public class UserStatistics : IUserStatistics
    {
        private readonly LibraryDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public UserStatistics(LibraryDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        
        public async Task Execute()
        {
            //var books = await _unitOfWork.BookRepository.Get();
            var users = await _context.Users.AsNoTracking().ToListAsync();

            // Получение самых популярных книг
            var popularBooks = await _context.Books
                .Include(x => x.Users)
                .Include(a => a.Author)
                .AsNoTracking()
                .Select(b => new
                {
                    Book = b,
                    BorrowedCount = b.Users.Count // Подсчет количества пользователей, взявших книгу
                })
                .OrderByDescending(b => b.BorrowedCount)
                .Take(10) // Берем только топ-10 популярных книг
                .ToListAsync();


            using (var package = new ExcelPackage())
            {
                // Лист с пользователями
                var userWorksheet = package.Workbook.Worksheets.Add("Users");
                userWorksheet.Cells[1, 1].Value = "ID";
                userWorksheet.Cells[1, 2].Value = "Name";
                userWorksheet.Cells[1, 3].Value = "Email";

                var userRow = 2;
                foreach (var user in users)
                {
                    userWorksheet.Cells[userRow, 1].Value = user.Id;
                    userWorksheet.Cells[userRow, 2].Value = user.UserName;
                    userWorksheet.Cells[userRow, 3].Value = user.Email;
                    userRow++;
                }

                // Лист с популярными книгами
                var bookWorksheet = package.Workbook.Worksheets.Add("Popular Books");
                bookWorksheet.Cells[1, 1].Value = "Title";
                bookWorksheet.Cells[1, 2].Value = "Author";
                bookWorksheet.Cells[1, 3].Value = "Borrowed Count";

                var bookRow = 2;
                foreach (var book in popularBooks)
                {
                    
                    bookWorksheet.Cells[bookRow, 1].Value = book.Book.Title;
                    bookWorksheet.Cells[bookRow, 2].Value = book.Book.Author.Surname;
                    bookWorksheet.Cells[bookRow, 3].Value = book.BorrowedCount;
                    bookRow++;
                }

                // Сохранение файла
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "LibraryStatistics.xlsx");
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);

                Console.WriteLine($"Файл успешно сохранен: {filePath}");
            }
        }
    }
}
