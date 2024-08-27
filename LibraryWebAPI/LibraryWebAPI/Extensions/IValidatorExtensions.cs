using FluentValidation.Results;

namespace LibraryWebAPI.Extensions
{
    public static class IValidatorExtensions
    {
        public static IEnumerable<object> GetErrorMessages(this ValidationResult result)
        {
            return result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
        }
    }
}
