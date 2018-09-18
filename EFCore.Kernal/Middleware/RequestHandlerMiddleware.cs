using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.WebCore.Middleware;

namespace EFCore.Kernal.Middleware
{
    public class RequestHandlerMiddleware : HandlerBaseMiddleware
    {
        public RequestHandlerMiddleware(RequestDelegate next)
            : base(next)
        {

        }

        protected override string InvokeContext(HttpContext context)
        {
            string title = context.Request.Query["title"];
            title = string.Format("Title of the report: {0}", title);
            return title;
        }

        protected override Encoding HttpEncoding => Encoding.UTF8;
    }
}