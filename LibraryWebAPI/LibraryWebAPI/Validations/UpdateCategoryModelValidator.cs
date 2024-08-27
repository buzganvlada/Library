using FluentValidation;
using LibraryWebAPI.Dto.Category;

namespace LibraryWebAPI.Validations
{
    public class UpdateCategoryModelValidator : AbstractValidator<CategoryDto>
    {
        public UpdateCategoryModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name maximum length is 50");
        }
    }
}

