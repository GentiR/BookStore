using FluentValidation;
using ProCredit.BookStore.Application.Requests;

namespace ProCredit.BookStore.Application.Validations;

public class SearchBooksRequestValidator : AbstractValidator<SearchBooksRequest>
{
    public SearchBooksRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
