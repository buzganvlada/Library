using FluentValidation;
using LibraryWebAPI.Dto.Category;

namespace LibraryWebAPI.Validations
{
    public class CategoryModelValidator : AbstractValidator<CreateCategoryDto>
    {

        public CategoryModelValidator()
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
