using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EFCore.Kernal.Extension
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

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer)
            where T : class
            => source.Distinct(new DynamicEqualityComparer<T>(comparer));

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