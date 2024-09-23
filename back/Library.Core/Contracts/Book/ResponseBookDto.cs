namespace Library.Core.Contracts.Book
{
    public class ResponseBookDto
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Guid GenreId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
