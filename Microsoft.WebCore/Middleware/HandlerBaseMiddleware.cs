using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.WebCore.Middleware
{
    /// <summary>
    /// do as the basic handler do
    /// </summary>

    public abstract class HandlerBaseMiddleware
    {
        protected HandlerBaseMiddleware(RequestDelegate next)
        {
        }

        public virtual async Task InvokeAsync(HttpContext context)
        {
            var text = InvokeContext(context);
            var token = Token ?? default(CancellationToken);

            if (HttpEncoding == null)
            {
                await context.Response.WriteAsync(text, token);
            }
            else
            {
                await context.Response.WriteAsync(text, HttpEncoding, token);
            }
        }

        protected abstract string InvokeContext(HttpContext context);
        protected abstract Encoding HttpEncoding { get; }
        protected virtual CancellationToken? Token { get; set; }
    }
}