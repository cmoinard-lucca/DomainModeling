using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DomainModeling
{
    public abstract class StructuralEqualityComparerBase<T> : IEqualityComparer<T>
    {
        private class ObjectOrCollectionEqualityComparer<TProp> : IEqualityComparer<TProp>
        {
            public bool Equals(TProp x, TProp y) => 
                InternalEquals(x, y);

            private bool InternalEquals(object? x, object? y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x != null && y == null || x == null)
                {
                    return false;
                }
                
                if (x is IDictionary dctX && y is IDictionary dctY)
                {
                    return DictionaryEquals(dctX, dctY);
                }
                
                if (x is IEnumerable enumerableX && y is IEnumerable enumerableY)
                {
                    return EnumerableEquals(enumerableX, enumerableY);
                }

                return Equals(x, y);
            }

            private bool DictionaryEquals(IDictionary dctX, IDictionary dctY)
            {
                return
                    dctX.Keys.Count == dctY.Keys.Count &&
                    dctX.Keys.Cast<object>()
                        .All(xKey => dctY.Keys.Cast<object>().Contains(xKey)) &&
                    dctX.Keys.Cast<object>()
                        .All(xKey => InternalEquals(dctX[xKey], dctY[xKey]));
            }

            private bool EnumerableEquals(IEnumerable enumerableX, IEnumerable enumerableY)
            {
                var xList = new List<object>(enumerableX.Cast<object>());
                var yList = new List<object>(enumerableY.Cast<object>());

                if (xList.Count != yList.Count)
                {
                    return false;
                }

                for (var i = 0; i < xList.Count; i++)
                {
                    var equals = InternalEquals(xList[i], yList[i]);
                    if (!equals)
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(TProp obj)
            {
                return obj?.GetHashCode() ?? 0;
            }
        }
        
        private readonly ObjectOrCollectionEqualityComparer<object> _equalityComparer =
            new ObjectOrCollectionEqualityComparer<object>();

        /// <summary>
        /// Returns all the values that determines the object equality
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityValues(T obj);

        public bool Equals(T x, T y) =>
            x == null && y == null ||
            x != null && y != null && GetEqualityValues(x).SequenceEqual(GetEqualityValues(y), _equalityComparer);

        private int InternalGetHashCode(object obj)
        {
            if (obj is IEnumerable)
            {
                return 0;
            }

            return obj?.GetHashCode() ?? 0;
        }

        private int ComputeHashCode(IEnumerable<object> objects) =>
            HashCode.Generate(objects, InternalGetHashCode);
        
        public int GetHashCode(T obj) =>
            ComputeHashCode(
                GetEqualityValues(obj));
    }
}