using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.Platform.Core.Extention
{
    [DebuggerStepThrough]
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// Orders a datasource by a property with the specified name in the specified direction
        /// for more details see http://msdn.microsoft.com/en-us/library/bb882637.aspx
        /// </summary>
        /// <param name="datasource">The datasource to order</param>
        /// <param name="propertyName">The name of the property to order by</param>
        /// <param name="direction">The direction</param>
        public static IQueryable<T> OrderBySpecifyProp<T>(this IQueryable<T> datasource, string propertyName, SortDirection direction)
        {
            if (string.IsNullOrEmpty(propertyName))
                return datasource;

            var type = typeof(T);
            var property = type.GetProperty(propertyName);

            if (property == null)
                throw new InvalidOperationException(string.Format("Could not find a property called '{0}' on type {1}",
                    propertyName, type));

            var parameter = Expression.Parameter(type, type.FullName);
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            var methodToInvoke = direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";

            var orderByCall = Expression.Call(typeof(Queryable),
                methodToInvoke,
                new[] { type, property.PropertyType },
                datasource.Expression,
                Expression.Quote(orderByExp));

            return datasource.Provider.CreateQuery<T>(orderByCall);
        }
    }
}