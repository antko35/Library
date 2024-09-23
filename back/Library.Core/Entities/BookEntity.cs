using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Entities
{
    public class BookEntity
    {
        public Guid Id {  get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Guid GenreId {  get; set; }
        public GenreEntity? Genre { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public AuthorEntity? Author { get; set; }
        public DateTime? BorrowDate { get; set; } 
        public DateTime? ReturnDate { get; set;}
    }
}
