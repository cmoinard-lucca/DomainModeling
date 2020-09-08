using System.Collections.Generic;
using System.Linq;
using DomainModeling.Exceptions;
using DomainModeling.Extensions;

namespace DomainModeling.Validations
{
    public interface IValidatable
    {
        IEnumerable<ValidationError> Validate();
    }

    public static class ValidatableExtensions
    {
        public static void EnsureIsValid(this IValidatable validatable)
        {
            var errors = validatable.Validate().ToList();
            if (errors.Any())
                throw new ValidationException(errors.ToNonEmptyList());
        }
    }
}