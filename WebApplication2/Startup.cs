using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Autofac;
using EFCore.Kernal;
using EFCore.Kernal.Extension;
using EFCore.Kernal.Middleware;
using EFCore.Kernal.ProviderModule;
using EFCore.Model;
using EFCore.Service.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EFCore.Infrustructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.WebCore.Middleware;
using Microsoft.WebCore.Middleware.Extension;
using HttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace WebApplication2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration.UseConfig();
            //            var cc = configuration["website:EnableLog4Net"];
            var webSiteSetting = GlobalSetting.GetConfigSection<WebSiteConfig>("WebSite");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SchoolContext>();
            //            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddWebsiteCrose();
            services.AddSessionCache();
//            services.AddMvc(opt => opt.Filters.Add(typeof(CustomExceptionFilterAttribute)));
            //                .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"); 

            services.AddMvc();

            return services.AddProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            //            TestProvider();

            app.UseCors(builder => builder.WithOrigins("http://localhost:8888").AllowAnyHeader().AllowAnyMethod());

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            //                        app.UseStaticHttpContext();
            app.UseSingleWebStaticFiles();
            app.UseCors("AllowSpecificOriginAndAllMethodAllHeader");
            app.UseSession();
            app.UseModule<RequestTestMiddleware>();
            app.UseModule<CacheStartTimeHeader>();

            // Create branch to the HandlerBaseMiddleware. 
            // All requests ending in .report will follow this branch.
            app.MapWhen(
                context => context.Request.Path.ToString().EndsWith(".report"),
                appBranch =>
                {
                    appBranch.UseHandler<RequestHandlerMiddleware>();
                }
                );


            //            var provider = app.ApplicationServices.GetService<IProviderManager>();
            app.UseMvcWithDefaultRoute();
            //            schoolContext.EnsureCreatedDataBase<SchoolContext, Student>();
            //            DbInitializer.Initialize(schoolContext);

            appLifetime.ApplicationStopped.Register(() => GlobalSetting.AppContainer.Dispose());
        }

        private static void TestProvider()
        {
            var schoolContext = GlobalSetting.AppContainer.Resolve<SchoolContext>();
            var uow = GlobalSetting.AppContainer.Resolve<IUnitOfWork>();
            var schoolUow = uow as UnitOfWork;
            var stdu = schoolContext.Student.Find(1);
            stdu.LastName = stdu.LastName + "++";
            stdu.LastName = stdu.LastName.Substring(0, stdu.LastName.Length - 1);
            schoolContext.SaveChanges();
            uow.BeginTransaction();
            uow.CommitTransaction();
            uow.RollBackTransaction();
            var fff = object.ReferenceEquals(schoolContext, schoolUow.dbContext);

            using (var scope = GlobalSetting.AppContainer.BeginLifetimeScope())
            {
                var sc = scope.Resolve<SchoolContext>();
                var scuow = scope.Resolve<IUnitOfWork>() as UnitOfWork;
                var fffs = object.ReferenceEquals(sc, scuow.dbContext);
            }
            using (var iocScope = ProviderManager.Instance.Scope())
            {
                var isc = iocScope.Resolve<SchoolContext>();
                var uowsc = iocScope.Resolve<IUnitOfWork>() as UnitOfWork;
                var fffsc = object.ReferenceEquals(isc, uowsc.dbContext);
            }
        }

        public class RequestTestMiddleware : ModuleBaseMiddleware
        {
            private IOptions<WebSiteConfig> sitConfig;
            private IValuesService svcService;
            public RequestTestMiddleware(RequestDelegate next, IOptions<WebSiteConfig> config,IValuesService value)
                : base(next)
            {
                this.sitConfig = config;
                svcService = value;
            }

            protected override HttpContext InvokeContext(HttpContext context)
            {
                string cultureQuery = context.Request.Query["culture"];
                if (!string.IsNullOrWhiteSpace(cultureQuery))
                {
                    var culture = new CultureInfo(cultureQuery);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
                var claims = new List<Claim>();
                claims.Add(new Claim("name","aa"));
                claims.Add(new Claim("role", "admin"));
                var identity = new ClaimsIdentity(claims,"claimLogin");
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;
                return context;
            }
        }
    }
}
