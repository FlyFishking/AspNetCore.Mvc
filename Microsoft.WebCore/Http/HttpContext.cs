using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.WebCore.Http
{
    public static class HttpContext
    {
        private static IHttpContextAccessor httpAccessor;

        public static AspNetCore.Http.HttpContext Current => httpAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            httpAccessor = accessor;
        }
    }

    public static class StaticHttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}
