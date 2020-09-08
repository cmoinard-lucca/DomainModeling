using System;
using System.Collections.Generic;
using System.Linq;
using DomainModeling.Exceptions;
using DomainModeling.Extensions;

namespace DomainModeling.Validations
{
    public static class Validation
    {
        
        
        public static Validation<T> Valid<T>(T value) =>
            new InternalValid<T>(value);

        public static Validation<T> Invalid<T>(NonEmptyReadOnlyList<ValidationError> errors) =>
            new InternalInvalid<T>(errors);

        public static Validation<T> Invalid<T>(ValidationError error, params ValidationError[] otherErrors) =>
            new InternalInvalid<T>(new NonEmptyReadOnlyList<ValidationError>(error, otherErrors));

        private class InternalValid<T> : Validation<T>
        {
            public InternalValid(T value)
            {
                Value = value;
            }

            public override bool IsValid => true;
            public override T Value { get; }

            public override NonEmptyReadOnlyList<ValidationError> Errors =>
                throw new NotSupportedException();

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Value!;
            }

            public override string ToString() => 
                $"Valid : {Value}";
        }

        private class InternalInvalid<T> : Validation<T>
        {
            private readonly NonEmptyReadOnlyList<ValidationError> _errors;

            public InternalInvalid(NonEmptyReadOnlyList<ValidationError> errors)
            {
                _errors = errors;
            }
            
            public override bool IsValid => false;

            public override T Value => throw new ValidationException(_errors);
            public override NonEmptyReadOnlyList<ValidationError> Errors => _errors;
            protected override IEnumerable<object> GetEqualityComponents()
            {
                return _errors;
            }

            public override string ToString()
            {
                var errors =
                    _errors
                        .Select(e => $"\t{e!}")
                        .JoinWith("\n");
                return $"Invalid ({Errors.Count} errors) :\n{errors}";
            }
        }
    }

    public abstract class Validation<T> : StructuralEqualityObject
    {
        public abstract bool IsValid { get; }
        public bool IsInvalid => !IsValid;
        public abstract T Value { get; }
        public abstract NonEmptyReadOnlyList<ValidationError> Errors { get; }

        public IReadOnlyCollection<ValidationError> SafeGetErrors() =>
            IsInvalid
                ? Errors.ToList()
                : new List<ValidationError>();
    }
}