namespace ProCredit.BookStore.Application.Requests;

public record SearchBooksRequest(
    string? Title,
    string? Author,
    int Page = 1,
    int PageSize = 10
);
