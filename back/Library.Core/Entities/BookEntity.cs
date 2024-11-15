using Library.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class BookEntity : Entity
    {
        // public Guid Id {  get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Guid GenreId { get; set; }
        public GenreEntity? Genre { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public AuthorEntity? Author { get; set; }
        public DateOnly? BorrowDate { get; set; }
        //public DateOnly? ReturnDate { get; set; }
        public string? CoverImagePath { get; set; } = string.Empty;

        //public Guid? UserId { get; set; }
        public ICollection<UserEntity?> Users { get; set; } = new List<UserEntity?>();
    }
}
