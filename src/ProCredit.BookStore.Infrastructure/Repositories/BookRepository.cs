using Microsoft.EntityFrameworkCore;
using ProCredit.BookStore.Application.DTOs;
using ProCredit.BookStore.Application.Interfaces.Repositories;
using ProCredit.BookStore.Application.ReadModels;
using ProCredit.BookStore.Application.Requests;
using ProCredit.BookStore.Domain;
using ProCredit.BookStore.Infrastructure.DbContexts;

namespace ProCredit.BookStore.Infrastructure.Repositories;

public class BookRepository(BookStoreDbContext dbContext) : IBookRepository
{
    #region CRUD Book
    public async Task<BookWithAuthorViewModel?> GetBookWithAuthorById(int bookId, CancellationToken cancellationToken)
    {
        return await (from book in dbContext.Books.AsNoTracking()
                        join author in dbContext.Authors.AsNoTracking() on book.AuthorId equals author.AuthorId
                        where book.BookId == bookId
                        select new BookWithAuthorViewModel
                        {
                            BookId = book.BookId,
                            AuthorId = author.AuthorId,
                            AuthorName = author.Name,
                            Title = book.Title,
                            SubTitle = book.SubTitle
                        })
                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Book?> GetBookById(int bookId, CancellationToken cancellationToken)
    {
        return await dbContext.Books
            .FirstOrDefaultAsync(x => x.BookId == bookId, cancellationToken);
    }

    public async Task<Book> AddBook(Book book, CancellationToken cancellationToken)
    {
        dbContext.Books.Add(book);
        await dbContext.SaveChangesAsync(cancellationToken);

        return book;
    }

    public async Task<bool> UpdateBook(Book book, CancellationToken cancellationToken)
    {
        var existingBook = await dbContext.Books
            .FirstOrDefaultAsync(x => x.BookId == book.BookId, cancellationToken);

        if (existingBook is null)
        {
            return false;
        }

        existingBook.AuthorId = book.AuthorId;
        existingBook.Title = book.Title;
        existingBook.SubTitle = book.SubTitle;

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteBook(int bookId, CancellationToken cancellationToken)
    {
        var book = await dbContext.Books.FirstOrDefaultAsync(x => x.BookId == bookId, cancellationToken);

        if (book is null)
        {
            return false;
        }

        dbContext.Books.Remove(book);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
    #endregion


    #region SearchBooks
    public async Task<IReadOnlyCollection<BookDTO>> GetBooks(SearchBooksRequest request, CancellationToken cancellationToken)
    {
        var query = SearchBooks(request);

        return await query.OrderBy(x => x.BookId)
                          .Skip((request.Page - 1) * request.PageSize)
                          .Take(request.PageSize)
                          .Select(x => new BookDTO
                            {
                                BookId = x.BookId,
                                Title = x.Title,
                                SubTitle = x.SubTitle,
                                Author = new AuthorDTO
                                {
                                    AuthorId = x.AuthorId,
                                    Name = x.AuthorName
                                }
                            })
                            .ToListAsync(cancellationToken);
    }

    private IQueryable<BookWithAuthorViewModel> SearchBooks(SearchBooksRequest request)
    {
        var query = from book in dbContext.Books.AsNoTracking()
                    join author in dbContext.Authors.AsNoTracking() on book.AuthorId equals author.AuthorId
                    select new BookWithAuthorViewModel
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        SubTitle = book.SubTitle,
                        AuthorId = author.AuthorId,
                        AuthorName = author.Name
                    };

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(x => x.Title.Contains(request.Title));
        }

        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            query = query.Where(x => x.AuthorName.Contains(request.Author));
        }

        return query;
    }

    public async Task<int> CountBooks(SearchBooksRequest request, CancellationToken cancellationToken)
    {
        return await SearchBooks(request).CountAsync(cancellationToken);
    }

    #endregion
}
