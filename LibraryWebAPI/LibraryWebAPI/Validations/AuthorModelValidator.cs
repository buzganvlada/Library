using FluentValidation;
using LibraryWebAPI.Dto.Author;

namespace LibraryWebAPI.Validations
{
    public class AuthorModelValidator : AbstractValidator<CreateAuthorDto>
    {

        public AuthorModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .NotNull()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name maximum length is 50");
        }
    }
}
