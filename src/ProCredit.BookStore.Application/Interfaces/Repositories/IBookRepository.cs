using ProCredit.BookStore.Application.DTOs;
using ProCredit.BookStore.Application.Params;
using ProCredit.BookStore.Application.ReadModels;
using ProCredit.BookStore.Application.Requests;
using ProCredit.BookStore.Domain;

namespace ProCredit.BookStore.Application.Interfaces.Repositories;

public interface IBookRepository
{
    Task<BookWithAuthorViewModel?> GetBookWithAuthorById(int bookId, CancellationToken cancellationToken);

    Task<Book?> GetBookById(int bookId, CancellationToken cancellationToken);

    Task<Book> AddBook(Book book, CancellationToken cancellationToken);

    Task<bool> UpdateBook(Book book, CancellationToken cancellationToken);

    Task<bool> DeleteBook(int bookId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BookDTO>> GetBooks(SearchBooksParams searchParams, CancellationToken cancellationToken);

    Task<int> CountBooks(SearchBooksParams searchParams, CancellationToken cancellationToken);
}
