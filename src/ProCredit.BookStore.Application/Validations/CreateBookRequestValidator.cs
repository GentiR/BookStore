using FluentValidation;
using ProCredit.BookStore.Application.Requests;

namespace ProCredit.BookStore.Application.Validations;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.AuthorId)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}
