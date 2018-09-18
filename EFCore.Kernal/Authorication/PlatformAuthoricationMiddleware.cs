using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EFCore.Kernal.Authorication
{
    public class PlatformAuthoricationMiddleware : AuthenticationMiddleware
    {
        public PlatformAuthoricationMiddleware(
            RequestDelegate next, 
            IAuthenticationSchemeProvider schemes) 
            : base(next, schemes)
        {
        }
    }
}
