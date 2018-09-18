﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using WebApplication4.Middleware.Platform;

namespace WebApplication4.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UsePlatformAuthentication(this IApplicationBuilder app) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<PlatformAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UsePlatformAuthentication(this IApplicationBuilder app, PlatformAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<PlatformAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
