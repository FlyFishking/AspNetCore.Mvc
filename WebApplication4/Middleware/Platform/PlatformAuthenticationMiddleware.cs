using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication4.Middleware.Platform.Events;
using WebApplication4.Services;

namespace WebApplication4.Middleware.Platform
{
    public class PlatformAuthenticationMiddleware : AuthenticationMiddleware
    {
        private readonly IPlatformService _platformService;

        public PlatformAuthenticationMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            IOptions<PlatformAuthenticationOptions> options,
            IPlatformService platformService,
            IAuthenticationSchemeProvider schemes)
            : base(next, schemes) {

            if (platformService == null) {
                throw new ArgumentNullException(nameof(platformService));
            }
            _platformService = platformService;

            if (dataProtectionProvider == null) {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }

            if (Options.Events == null) {
                Options.Events = new PlatformAuthenticationEvents();
            }
            if (string.IsNullOrEmpty(Options.PlatformName)) {
                Options.PlatformName = PlatformAuthenticationDefaults.PlatformPrefix + Options.AuthenticationScheme;
            }
            if (Options.TicketDataFormat == null) {

                //此处应该和平台约定一套加密解密的算法，然后实现掉IDataProtectionProvider接口
                var provider = Options.DataProtectionProvider ?? dataProtectionProvider;
                var dataProtector = provider.CreateProtector(typeof(PlatformAuthenticationMiddleware).FullName, Options.AuthenticationScheme, "v2");
                Options.TicketDataFormat = new TicketDataFormat(dataProtector);
            }
            if (Options.CookieManager == null) {
                Options.CookieManager = new ChunkingCookieManager();
            }
            if (!Options.LogoutPath.HasValue) {
                Options.LogoutPath = PlatformAuthenticationDefaults.LogoutPath;
            }
            if (!Options.AccessDeniedPath.HasValue) {
                Options.AccessDeniedPath = PlatformAuthenticationDefaults.AccessDeniedPath;
            } 
        }

        protected override AuthenticationHandler<PlatformAuthenticationOptions> CreateHandler() {
            return new PlatformAuthenticationHandler(_platformService);
        }
    }
}
