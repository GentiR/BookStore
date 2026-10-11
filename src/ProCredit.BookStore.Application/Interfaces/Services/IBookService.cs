using ProCredit.BookStore.Application.DTOs;
using ProCredit.BookStore.Application.Requests;
using ProCredit.BookStore.Application.Results;

namespace ProCredit.BookStore.Application.Interfaces.Services;

public interface IBookService
{
    Task<BookDTO?> GetBookWithAuthorById(int bookId, CancellationToken cancellationToken);

    Task<BookOperationResult> AddBook(CreateBookRequest request, CancellationToken cancellationToken);

    Task<BookOperationResult> UpdateBook(UpdateBookRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteBook(int bookId, CancellationToken cancellationToken);

    Task<PagedResultDTO<BookDTO>> GetBooks(SearchBooksRequest request, CancellationToken cancellationToken);
}
