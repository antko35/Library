using Library.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class AuthorEntity : Entity
    {
        // public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public List<BookEntity> Books { get; set; } = new List<BookEntity>();
    }
}
