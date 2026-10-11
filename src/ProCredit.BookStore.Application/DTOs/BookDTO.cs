namespace ProCredit.BookStore.Application.DTOs;

public class BookDTO
{
    public int BookId { get; set; }

    public required AuthorDTO Author { get; set; }

    public required string Title { get; set; }

    public string? SubTitle { get; set; }
}