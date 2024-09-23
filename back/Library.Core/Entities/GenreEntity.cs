namespace Library.Persistence.Entities
{
    public class GenreEntity
    {
        public Guid Id { get; set; }
        public string Genre { get; set; } = string.Empty;
        public List<BookEntity> Books { get; set; } = new List<BookEntity>();
    }
}
