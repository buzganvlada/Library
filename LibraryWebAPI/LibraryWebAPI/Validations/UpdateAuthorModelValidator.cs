using FluentValidation;
using LibraryWebAPI.Dto.Author;

namespace LibraryWebAPI.Validations
{
    public class UpdateAuthorModelValidator : AbstractValidator<AuthorDto>
    {
        public UpdateAuthorModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name maximum length is 50");
        }
    }
}
