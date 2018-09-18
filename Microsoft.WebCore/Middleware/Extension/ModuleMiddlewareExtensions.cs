using System;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.WebCore.Middleware.Extension
{
    public static class ModuleMiddlewareExtensions
    {
        public static IApplicationBuilder UseModule<TModule>(this IApplicationBuilder builder)
            where TModule : ModuleBaseMiddleware
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.UseMiddleware<TModule>();
        }
    }
}