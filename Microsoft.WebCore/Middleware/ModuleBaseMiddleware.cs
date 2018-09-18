using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.WebCore.Middleware
{
    /// <summary>
    /// do as the basic module do
    /// </summary>
    public abstract class ModuleBaseMiddleware
    {
        private readonly RequestDelegate next;
        protected ModuleBaseMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public virtual async Task InvokeAsync(HttpContext context)
        {
            context = InvokeContext(context);

            // Call the next delegate/middleware in the pipeline
            //            await next(context);
            await next.Invoke(context);
        }

        protected abstract HttpContext InvokeContext(HttpContext context);
    }
}