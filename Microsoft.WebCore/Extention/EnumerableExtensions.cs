using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.WebCore.Extention
{
    [DebuggerStepThrough]
    public static class EnumerableExtensions
    {
        /// <summary>
        ///  Concatenates the members of a collection, using the specified separator between each member
        /// </summary>
        /// <param name="source">A collection that contains the objects to concatenate.</param>
        /// <param name="separator">
        ///     The string to use as a separator.separator is included in the returned string
        ///     only if values has more than one element.
        /// </param>
        /// <returns>
        ///     A string that consists of the members of values delimited by the separator string.
        ///     If values has no members, the method returns System.String.Empty.
        /// </returns>
        public static string JoinString(this IEnumerable<object> source, string separator = ", ")
            => string.Join(separator, source);

        public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
            => condition ? source.Where(predicate) : source;

        /// <summary>
        /// Orders a datasource by a property with the specified name in the specified direction
        /// </summary>
        /// <param name="source">The datasource to order</param>
        /// <param name="propertyName">The name of the property to order by</param>
        /// <param name="direction">The direction</param>
        public static IEnumerable<T> OrderBySpecifyProp<T>(this IEnumerable<T> source, string propertyName, SortDirection direction)
        {
            return source.AsQueryable().OrderBySpecifyProp(propertyName, direction);
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer)
            where T : class
            => source.Distinct(new DynamicEqualityComparer<T>(comparer));

        /// <summary>
        ///  Groups the elements of a sequence according to a specified key selector function
        /// and select the first elements in each group.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey">Key for group by</typeparam>
        /// <param name="source">The type of the key returned by keySelector.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            => source.GroupBy(keySelector).Select(group => group.First());

        private sealed class DynamicEqualityComparer<T> : IEqualityComparer<T>
            where T : class
        {
            private readonly Func<T, T, bool> func;

            public DynamicEqualityComparer(Func<T, T, bool> func)
            {
                this.func = func;
            }

            public bool Equals(T x, T y) => func(x, y);

            public int GetHashCode(T obj) => 0; // force Equals
        }
    }
}