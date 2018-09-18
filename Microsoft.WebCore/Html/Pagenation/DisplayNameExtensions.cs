
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace XinDaYang.RentalCars.Tools.Pagenation
{
    public static class DisplayNameExtensions
    {
        public static IHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return GetDisplayName(expression);
        }

        public static IHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<PagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return GetDisplayName(expression);
        }

        private static IHtmlString GetDisplayName<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string resolvedDisplayName = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            return new MvcHtmlString(HttpUtility.HtmlEncode(resolvedDisplayName));
        }
    }
}
