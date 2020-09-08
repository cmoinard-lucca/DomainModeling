using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainModeling.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> source) =>
            !source.Any();
        
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            !source.Any(predicate);
        
        public static NonEmptyReadOnlyList<T> ToNonEmptyList<T>(this IEnumerable<T> source) =>
            new NonEmptyReadOnlyList<T>(source);
    }
}