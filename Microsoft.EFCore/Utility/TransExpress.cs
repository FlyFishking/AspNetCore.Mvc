using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.EFCore.Utility
{
    /// <summary>
    /// 将两个对象中相同字段的值拷贝到目标<see cref="TOut"/>对象
    /// <example>
    /// <![CDATA[
    ///     var cc = TransExpress<a, b>.Clone(new a { age = 1, c = 2, dd = 3.0f, name = "fds", time = DateTime.Now });
    /// ]]>
    /// </example>
    /// </summary>
    /// <typeparam name="TIn">源对象</typeparam>
    /// <typeparam name="TOut">目标对象</typeparam>
    public static class TransExpress<TIn, TOut>
    {
        /// <summary>
        /// 将两个对象中相同字段的值拷贝到目标<see cref="TOut"/>对象
        /// <example>
        /// <![CDATA[
        ///     var cc = TransExpress<a, b>.Clone(new a { age = 1, c = 2, dd = 3.0f, name = "fds", time = DateTime.Now });
        /// ]]>
        /// </example>
        /// </summary>
        /// <typeparam name="TIn">源对象</typeparam>
        /// <typeparam name="TOut">目标对象</typeparam>
        public static TOut Clone(TIn copeFrom)
        {
            return cache(copeFrom);
        }

        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            var typeIn = typeof(TIn);
            var expressParameter = Expression.Parameter(typeIn, typeIn.FullName);

            var memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite) continue;
                var tInProperty = typeof(TIn).GetProperty(item.Name);
                if (tInProperty != null && tInProperty.CanRead && tInProperty.PropertyType.FullName == item.PropertyType.FullName)
                {
                    var property = Expression.Property(expressParameter, tInProperty);
                    var memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
            }

            var memberInitExpress = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            //        var memberInitExpress = Expression.MemberInit(Expression.New(typeof(TOut)), (from item in typeof (TOut).GetProperties() where item.CanWrite let tInProperty = typeof (TIn).GetProperty(item.Name) where tInProperty != null && tInProperty.CanRead && tInProperty.PropertyType.FullName == item.PropertyType.FullName let property = Expression.Property(expressParameter, tInProperty) select Expression.Bind(item, property)).Cast<MemberBinding>().ToArray());

            var lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpress, expressParameter);
            return lambda.Compile();
        }

    }
}
