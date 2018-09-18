using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Middleware.Platform.Events
{
    public class PlatformRedirectContext : BasePlatformContext
    {
        public PlatformRedirectContext(ActionContext context, PlatformAuthenticationOptions options, string redirectUri, AuthenticationProperties properties)
            : base(context, options)
        {
            RedirectUri = redirectUri;
            Properties = properties;
        }

        public string RedirectUri { get; set; }

        public AuthenticationProperties Properties { get; }
    }
}
