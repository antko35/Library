using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.Book
{
    public class RequestUpdateBookDto
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Guid GenreId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
    }
}
