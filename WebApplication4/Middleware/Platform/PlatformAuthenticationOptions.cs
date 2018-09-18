using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebApplication4.Middleware.Platform.Events;

namespace WebApplication4.Middleware.Platform
{
    public class PlatformAuthenticationOptions : AuthenticationSchemeOptions, IOptions<PlatformAuthenticationOptions>
    {
        private string _PlatformName;

        public PlatformAuthenticationOptions() {
//            DefaultAuthenticateScheme = PlatformAuthenticationDefaults.AuthenticationScheme;
            ReturnUrlParameter = PlatformAuthenticationDefaults.ReturnUrlParameter;
            ExpireTimeSpan = TimeSpan.FromDays(14);
            SlidingExpiration = true;
            PlatformHttpOnly = true;
            Events = new PlatformAuthenticationEvents();
        }

        public string PlatformName {
            get { return _PlatformName; }
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                _PlatformName = value;
            }
        }
        public string PlatformDomain { get; set; }
        public string PlatformPath { get; set; }
        public bool PlatformHttpOnly { get; set; }
        public IDataProtectionProvider DataProtectionProvider { get; set; }
        public TimeSpan ExpireTimeSpan { get; set; }
        public bool SlidingExpiration { get; set; }
        public PathString LoginPath { get; set; }
        public PathString LogoutPath { get; set; }
        public PathString AccessDeniedPath { get; set; }
        public string ReturnUrlParameter { get; set; }
        public IPlatformAuthenticationEvents Events { get; set; }
        public ISecureDataFormat<AuthenticationTicket> TicketDataFormat { get; set; }
        public Microsoft.AspNetCore.Authentication.Cookies.ICookieManager CookieManager { get; set; }
        public IUserSessionStore UserSessionStore { get; set; }
        PlatformAuthenticationOptions IOptions<PlatformAuthenticationOptions>.Value {
            get {
                return this;
            }
        }
    }
}
