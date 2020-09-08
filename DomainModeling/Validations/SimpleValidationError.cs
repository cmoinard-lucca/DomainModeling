using System.Collections.Generic;

namespace DomainModeling.Validations
{
    public abstract class SimpleValidationError : ValidationError
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}