using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication4.Middleware.Platform.Events
{
    public class BasePlatformContext : PageContext
    {
        public BasePlatformContext(
            ActionContext context,
            PlatformAuthenticationOptions options)
            : base(context)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            Options = options;
        }

        public HttpRequest Request => HttpContext.Request;
        public HttpResponse Response => HttpContext.Response;
        public PlatformAuthenticationOptions Options { get; }
    }
}
