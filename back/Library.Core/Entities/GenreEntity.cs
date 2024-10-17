using Library.Core.Entities;

namespace Library.Persistence.Entities
{
    public class GenreEntity : Entity
    {
        //public Guid Id { get; set; }
        public string Genre { get; set; } = string.Empty;
        public List<BookEntity> Books { get; set; } = new List<BookEntity>();
    }
}
