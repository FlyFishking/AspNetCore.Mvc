using EFCore.Kernal;
using EFCore.Kernal.Filter;
using EFCore.Kernal.ProviderModule;
using EFCore.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EFCore.Infrustructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace WebApplication4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            GlobalSetting.SetConfiguration(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SchoolContext>(
                opt => opt.UseDataBaseBuilder(GlobalSetting.Configuration.GetConnectionString("MySqlConnection"), "mysql"));
            services.AddScoped<DbContext>(provider => provider.GetService<SchoolContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //            services.AddCors();
            services.AddMvc(opt =>
                {
                    opt.Filters.Add(typeof(CustomExceptionAttribute));
//                    opt.UseCentralRoutePrefix(new RouteAttribute("api/v{version}"));
                })
                .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");

            services.AddMvc();
//            services.AddMvcCore().AddJsonFormatters();

            var svcProvider = ProviderManager.Instance.Initialize(services);
            GlobalSetting.SetContainer(ProviderManager.Instance.Container);
            return svcProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //注册PlatformAuthentication中间件
            //            app.UsePlatformAuthentication(new PlatformAuthenticationOptions()
            //            {
            //                UserSessionStore = new UserSessionStore(),
            //            });

            app.UseMvc();
        }
    }

    public static class MvcOptionsExtensions
    {
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
    }

    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel centralPrefix;

        public RouteConvention(IRouteTemplateProvider provider)
        {
            centralPrefix = new AttributeRouteModel(provider);
        }

        public void Apply(ApplicationModel application)
        {
            //all Controllers in the app
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    matchedSelectors.ForEach(t =>
                    {
                        // add [centralPrefix] befor the route path
                        t.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(centralPrefix, t.AttributeRouteModel);
                    });
                }

                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    unmatchedSelectors.ForEach(t => t.AttributeRouteModel = centralPrefix);
                }
            }
        }
    }
}
