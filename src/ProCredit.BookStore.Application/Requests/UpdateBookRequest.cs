using ProCredit.BookStore.Application.DTOs;

namespace ProCredit.BookStore.Application.Requests;

public record UpdateBookRequest
(
    int BookId,
    int AuthorId,
    string Title,
    string? SubTitle
);
