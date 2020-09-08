using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainModeling
{
    public abstract class StructuralEqualityObject
    {
        private readonly Lazy<int> _lazyHashCode;

        protected StructuralEqualityObject()
        {
            _lazyHashCode = new Lazy<int>(() => HashCode.GenerateStructural(GetEqualityComponents()));
        }
        
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            var other = (StructuralEqualityObject) obj;

            return
                GetEqualityComponents()
                    .SequenceEqual(
                        other.GetEqualityComponents(),
                        StructuralEqualityComparer.Instance);
        }
        
        public override int GetHashCode() => _lazyHashCode.Value;
        
        public static bool operator ==(StructuralEqualityObject a, StructuralEqualityObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(StructuralEqualityObject a, StructuralEqualityObject b)
        {
            return !(a == b);
        }
    }
}