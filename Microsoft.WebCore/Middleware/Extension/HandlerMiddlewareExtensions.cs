using System;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.WebCore.Middleware.Extension
{
    /// <summary>
    /// call like this
    /// <![CDATA[
    ///     // Create branch to the HandlerBaseMiddleware. 
    ///     // All requests ending in .report will follow this branch.
    ///     app.MapWhen(
    ///     context => context.Request.Path.ToString().EndsWith(".report"),
    ///     appBranch => {
    ///     // ... optionally add more middleware to this branch
    ///     appBranch.UseHandler<RequestHandlerMiddleware>();
    ///     });
    /// ]]>
    /// </summary>

    public static class HandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseHandler<THandler>(this IApplicationBuilder builder)
            where THandler : HandlerBaseMiddleware
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.UseMiddleware<THandler>();
        }
    }
}