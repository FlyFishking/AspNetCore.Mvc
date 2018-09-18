using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.WebCore.Middleware;

namespace EFCore.Kernal.Middleware
{
    public class CacheStartTimeHeader : ModuleBaseMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDistributedCache disCache;
        private readonly IMemoryCache memoryCache;
        public CacheStartTimeHeader(RequestDelegate next, IDistributedCache disCache, IMemoryCache memoryCache)
            : base(next)
        {
            this.next = next;
            this.disCache = disCache;
            this.memoryCache = memoryCache;
        }

        protected override HttpContext InvokeContext(HttpContext context)
        {
//            var memoryVal = memoryCache.Get("_Entry");
            var startTimeString = "Not found.";
            var value = disCache.Get("lastServerStartTime");
            if (value != null)
            {
                startTimeString = Encoding.UTF8.GetString(value);
            }

            context.Response.Headers.Append("Last-Server-Start-Time", startTimeString);
            return context;
        }
    }
}