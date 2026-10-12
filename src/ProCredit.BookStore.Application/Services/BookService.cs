using Mapster;
using ProCredit.BookStore.Application.DTOs;
using ProCredit.BookStore.Application.Interfaces.Repositories;
using ProCredit.BookStore.Application.Interfaces.Services;
using ProCredit.BookStore.Application.Params;
using ProCredit.BookStore.Application.Requests;
using ProCredit.BookStore.Application.Results;
using ProCredit.BookStore.Domain;

namespace ProCredit.BookStore.Application.Services;

public class BookService(
                        IBookRepository bookRepository,
                        IAuthorRepository authorRepository) : IBookService
{
    public async Task<BookDTO?> GetBookWithAuthorById(int bookId, CancellationToken cancellationToken)
    { 
        var book = await bookRepository.GetBookWithAuthorById(bookId, cancellationToken);

        if (book is null)
        {
            return null;
        }

        return new BookDTO
        {
            BookId = book.BookId,
            Author = new AuthorDTO
            {
                AuthorId = book.AuthorId,
                Name = book.AuthorName
            },
            Title = book.Title,
            SubTitle = book.SubTitle
        };
    }

    public async Task<BookOperationResult> AddBook(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetAuthorById(request.AuthorId, cancellationToken);

        if (author is null)
        {
            return BookOperationResult.AuthorNotFound;
        }

        var book = new Book
        {
            AuthorId = request.AuthorId,
            Title = request.Title,
            SubTitle = request.SubTitle
        };

        await bookRepository.AddBook(book, cancellationToken);

        return BookOperationResult.Success;
    }

    public async Task<BookOperationResult> UpdateBook(UpdateBookRequest request, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetAuthorById(request.AuthorId, cancellationToken);

        if (author is null)
        {
            return BookOperationResult.AuthorNotFound;
        }

        var book = new Book
        {
            BookId = request.BookId,
            AuthorId = request.AuthorId,
            Title = request.Title,
            SubTitle = request.SubTitle
        };

        var updated = await bookRepository.UpdateBook(book, cancellationToken);

        return updated ? BookOperationResult.Success : BookOperationResult.BookNotFound;
    }

    public async Task<bool> DeleteBook(int bookId, CancellationToken cancellationToken)
    {
        return await bookRepository.DeleteBook(bookId, cancellationToken);
    }

    public async Task<BookSearchResultDTO> GetBooks(SearchBooksRequest request, CancellationToken cancellationToken)
    {
        var searchParams = new SearchBooksParams
        {
            Title = request.Title,
            Author = request.Author,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var books = await bookRepository.GetBooks(searchParams, cancellationToken);
        var totalCount = await bookRepository.CountBooks(searchParams, cancellationToken);

        return new BookSearchResultDTO
        {
            Items = books,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
