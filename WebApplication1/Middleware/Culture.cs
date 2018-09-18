using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Middleware.Culture
{
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string key) where T : class
        {
            byte[] byteArray = null;
            if (session.TryGetValue(key, out byteArray))
            {
                using (var memoryStream = new MemoryStream(byteArray))
                {
                    var obj = ProtoBuf.Serializer.Deserialize<T>(memoryStream);
                    return obj;
                }
            }
            return null;
        }

        public static void Set<T>(this ISession session, string key, T value) where T : class
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(memoryStream, value);
                    byte[] byteArray = memoryStream.ToArray();
                    session.Set(key, byteArray);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

            }

            // Call the next delegate/middleware in the pipeline
            return this.next(context);
        }
    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
