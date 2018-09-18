using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.WebCore.Html
{
    /// <summary>
    /// Extension methods on IEnumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts the source sequence into an IEnumerable of SelectListItem
        /// </summary>
        /// <param name="items">Source sequence</param>
        /// <param name="nameSelector">Lambda that specifies the name for the SelectList items</param>
        /// <param name="valueSelector">Lambda that specifies the value for the SelectList items</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector)
        {
            return items.ToSelectList(valueSelector, nameSelector, x => false);
        }

        /// <summary>
        /// Converts the source sequence into an IEnumerable of SelectListItem
        /// </summary>
        /// <param name="items">Source sequence</param>
        /// <param name="nameSelector">Lambda that specifies the name for the SelectList items</param>
        /// <param name="valueSelector">Lambda that specifies the value for the SelectList items</param>
        /// <param name="selectedItems">Those items that should be selected</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, IEnumerable<TValue> selectedItems)
        {
            return items.ToSelectList(valueSelector, nameSelector, x => selectedItems != null && selectedItems.Contains(valueSelector(x)));
        }

        /// <summary>
        /// Converts the source sequence into an IEnumerable of SelectListItem
        /// </summary>
        /// <param name="items">Source sequence</param>
        /// <param name="nameSelector">Lambda that specifies the name for the SelectList items</param>
        /// <param name="valueSelector">Lambda that specifies the value for the SelectList items</param>
        /// <param name="selectedValueSelector">Lambda that specifies whether the item should be selected</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, bool> selectedValueSelector)
        {
            foreach (var item in items)
            {
                var value = valueSelector(item);

                yield return new SelectListItem
                {
                    Name = nameSelector(item),
                    Value = value.ToString(),
                    Selected = selectedValueSelector(item)
                };
            }
        }
    }
}