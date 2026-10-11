using ProCredit.BookStore.Application.DTOs;

namespace ProCredit.BookStore.Application.Requests;

public record CreateBookRequest
(
    int AuthorId,
    string Title,
    string? SubTitle
);
