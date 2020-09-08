using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DomainModeling.Extensions;

namespace DomainModeling
{
    public interface INonEmptyReadOnlyList<out T> : IReadOnlyList<T>
    {
    }
    
    public class NonEmptyReadOnlyList<T> : INonEmptyReadOnlyList<T>
    {
        private readonly List<T> _internalList;

        public NonEmptyReadOnlyList(T value, params T[] values)
        {
            _internalList = 
                new [] {value}
                    .Concat(values)
                    .ToList();
        }

        public NonEmptyReadOnlyList(IEnumerable<T> values)
        {
            var valueList = values.ToList();
            if (valueList.None())
                throw new ArgumentException("Cannot be empty");
            
            _internalList = valueList.ToList();
        }

        public IEnumerator<T> GetEnumerator() => _internalList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public int Count => _internalList.Count;

        public T this[int index] => _internalList[index];
    }
}