using FluentValidation;
using LibraryWebAPI.Dto.Book;

namespace LibraryWebAPI.Validations
{
    public class BookModelValidator : AbstractValidator<CreateBookDto>
    {

        public BookModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(50)
                .WithMessage("Title maximum length is 50");
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("Invalid author id");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Invalid category id");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Invalid Price");
        }
    }
}
