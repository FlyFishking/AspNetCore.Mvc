using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Middleware.Platform.Events
{
    public class PlatformSigningOutContext : BasePlatformContext
    {
        public PlatformSigningOutContext(
            ActionContext context, 
            PlatformAuthenticationOptions options, 
            AuthenticationProperties properties,
            CookieOptions cookieOptions)
            : base(context, options)
        {
            CookieOptions = cookieOptions;
            Properties = properties;
        }

        public CookieOptions CookieOptions { get; set; }

        public AuthenticationProperties Properties { get; set; }
    }
}
