using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Middleware.Platform.Events
{
    public class PlatformValidatePrincipalContext : BasePlatformContext
    {
        public PlatformValidatePrincipalContext(ActionContext context, AuthenticationTicket ticket, PlatformAuthenticationOptions options)
            : base(context, options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Principal = ticket.Principal;
            Properties = ticket.Properties;
        }

        public ClaimsPrincipal Principal { get; private set; }

        public AuthenticationProperties Properties { get; private set; }

        public bool ShouldRenew { get; set; }
    }
}
