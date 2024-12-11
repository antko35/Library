namespace Library.Application.Contracts.Book
{
    public class ResponseBookDto
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool InProfile { get; set; }
        public Guid GenreId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public DateOnly? BorrowDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string? CoverImagePath { get; set; } = string.Empty;
    }
}
