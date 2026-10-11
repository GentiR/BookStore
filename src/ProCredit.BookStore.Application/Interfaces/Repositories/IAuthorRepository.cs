using ProCredit.BookStore.Domain;

namespace ProCredit.BookStore.Application.Interfaces.Repositories;

public interface IAuthorRepository
{
    Task<Author?> GetAuthorById(int authorId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Author>> GetAuthors(CancellationToken cancellationToken);

    Task<Author> AddAuthor(Author author, CancellationToken cancellationToken);
}