using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace WebApplication1.Middleware
{
    public class RequestSetOptionsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IOptions<AppOptions> injectedOptions;

        public RequestSetOptionsMiddleware(RequestDelegate next, IOptions<AppOptions> injectedOptions)
        {
            this.next = next;
            this.injectedOptions = injectedOptions;
        }

        public async Task Invoke(HttpContext httpContext)
        {
#if debug

            Console.WriteLine("RequestSetOptionsMiddleware.Invoke");
#endif

            var option = httpContext.Request.Query["option"];

            if (!string.IsNullOrWhiteSpace(option))
            {
                this.injectedOptions.Value.Option = WebUtility.HtmlEncode(option);
            }

            await this.next(httpContext);
        }
    }

    public class AppOptions
    {
        public string Option { get; set; } = "Option Default Value";
    }

    public class RequestSetOptionsStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<RequestSetOptionsMiddleware>();
                next(builder);
            };
        }
    }
}
