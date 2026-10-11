namespace ProCredit.BookStore.Application.ReadModels;

public class BookWithAuthorViewModel
{
    public int BookId { get; set; }

    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? SubTitle { get; set; }
}