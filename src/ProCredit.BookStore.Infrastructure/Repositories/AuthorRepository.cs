using Microsoft.EntityFrameworkCore;
using ProCredit.BookStore.Application.Interfaces.Repositories;
using ProCredit.BookStore.Domain;
using ProCredit.BookStore.Infrastructure.DbContexts;

namespace ProCredit.BookStore.Infrastructure.Repositories;

public class AuthorRepository(BookStoreDbContext dbContext) : IAuthorRepository
{
    public async Task<Author?> GetAuthorById(int authorId, CancellationToken cancellationToken)
    {
        return await dbContext.Authors.AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.AuthorId == authorId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Author>> GetAuthors(CancellationToken cancellationToken)
    {
        return await dbContext.Authors.AsNoTracking()
                                      .OrderBy(x => x.AuthorId)
                                      .ToListAsync(cancellationToken);
    }

    public async Task<Author> AddAuthor(Author author, CancellationToken cancellationToken)
    {
        dbContext.Authors.Add(author);
        await dbContext.SaveChangesAsync(cancellationToken);

        return author;
    }
}