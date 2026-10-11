namespace ProCredit.BookStore.Application.DTOs;

public class PagedResultDTO<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages => PageSize <= 0
        ? 0
        : (int)Math.Ceiling((double)TotalCount / PageSize);
}