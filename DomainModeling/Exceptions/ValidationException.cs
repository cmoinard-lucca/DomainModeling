using System;
using DomainModeling.Validations;

namespace DomainModeling.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(ValidationError error)
            : this(new NonEmptyReadOnlyList<ValidationError>(error))
        {
        }
        
        public ValidationException(NonEmptyReadOnlyList<ValidationError> errors)
        {
            Errors = errors;
        }

        public NonEmptyReadOnlyList<ValidationError> Errors { get; }
    }
}