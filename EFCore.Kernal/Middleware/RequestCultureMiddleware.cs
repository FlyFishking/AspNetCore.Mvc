using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.WebCore.Middleware;

namespace EFCore.Kernal.Middleware
{
    public class RequestCultureMiddleware : ModuleBaseMiddleware
    {
        public RequestCultureMiddleware(RequestDelegate next)
            : base(next)
        {
        }

        protected override HttpContext InvokeContext(HttpContext context)
        {
            string cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            return context;
        }
    }
}