using Autofac;
using Autofac.Extensions.DependencyInjection;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System;
using System.Globalization;
using System.IO;
using WebApplication1.Data;
using WebApplication1.JsonSetting;
using WebApplication1.Middleware;
using WebApplication1.Middleware.Culture;
using WebApplication1.Pages;

namespace WebApplication1
{
    public class Startup
    {
        public readonly GloableSetting setting;
        //        public IConfiguration Configuration { get; }
        //        public IContainer ApplicationContainer { get; private set; }
        //        public static ILoggerRepository LoggerRepository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration">IConfiguration to configure the app during startup.</param>
        public Startup(IConfiguration configuration)
        {
            //            Configuration = configuration;
            setting = new GloableSetting(configuration);

            //Json ������Ϣӳ�䵽 AppSettings ��
            var appSettingMapping = configuration.GetSection("App").Get<AppSettings>();

            //log4net config
            var logger = LogManager.CreateRepository("NETCoreLoggerRepository");
            var file = new FileInfo("log4net.config");
            XmlConfigurator.Configure(logger, file);
            //            LoggerRepository = logger;
            setting.SetLoggerRepo(logger);
        }

        public ServiceProvider provider { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            provider = services.BuildServiceProvider();
            //            services.AddDbContext<SchoolContext>(options =>
            //                options.UseSqlServer(Configuration.GetConnectionString("SchoolConnectionString")));
            services.AddDbContext<SchoolContext>(options =>
                options.UseMySQL(GloableSetting.Configuration.GetConnectionString("MySqlConnection")));
            //            MySQLDataBaseConfig.CreateContext();

            //�Զ����м��
            services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();
            //            services.AddScoped<IStartupFilter, RequestSetOptionsStartupFilter>();
            //            services.AddSingleton<IStartupFilter, RequestSetOptionsStartupFilter>();

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddMvc(options =>
            {
                //���ģ�Ͱ󶨹�����
                //                options.Filters.Add(typeof(ModelValidateActionFilter));

                //�����Ȩ���������Ա�ǿ��ִ��Authentication��ת�������߼�
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //var policy = new AuthorizationPolicyBuilder().AddRequirements(new AuthenticationRequirement()).Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddMvc()
                .AddMvcOptions(options =>
                {
                    // add actionfilter 1
                    //                    var provider = services.BuildServiceProvider();
                    //                    var filter = provider.GetService<ExceptionFilter>();
                    //                    options.Filters.Add(filter);
                    // add actionfilter 2
                    //                    options.Filters.Add<ActionFilter>();
                });
            //                // Include the 2.1 behaviors
            //                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            //                // Except for the following.
            //                .AddMvcOptions(options =>
            //                {
            //                    // Don't combine authorize filters (keep 2.0 behavior).
            //                    options.AllowCombiningAuthorizeFilters = false;
            //                    // All exceptions thrown by an IInputFormatter will be treated
            //                    // as model state errors (keep 2.0 behavior).
            //                    options.InputFormatterExceptionPolicy =
            //                        InputFormatterExceptionPolicy.AllExceptions;
            //                });
            // Create the autofac container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            //
            // Note that Populate is basically a foreach to add things
            // into Autofac that are in the collection. If you register
            // things in Autofac BEFORE Populate then the stuff in the
            // ServiceCollection can override those things; if you register
            // AFTER Populate those registrations can override things
            // in the ServiceCollection. Mix and match as needed.
            builder.RegisterModule(new AutofacModule());

            builder.RegisterType<IndexModel>().PropertiesAutowired();
            builder.Populate(services);
            var container = builder.Build();

            //            this.ApplicationContainer = container;
            setting.SetContainer(container);
            return new AutofacServiceProvider(container);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env">IHostingEnvironment to configure services by environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var log = LogManager.GetLogger(GloableSetting.LoggerRepository.Name, typeof(Startup));
            log.Info("test");

            var ccc = GloableSetting.AppContainer.Resolve<IDiTest>();
            var bb = ccc as DiTest;

            using (var p = provider.CreateScope())
            {
                
            }

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            //            app.UseMiddleware<RequestSetOptionsStartupFilter>();
            app.UseRequestCulture();

            //app.Use   app.Map   app.Run ����
            //1��ִ�йܵ�֮����� next.Invoke() ������������ִ�к����ܵ���ҵ
            //2��ͨ����ַӳ�䣬��ת����ͬ�Ĵ����߼���Ԫ����� HandleMapTest1��HandleMapTest2
            //3��app.Run �����ܵ���������������ܵ�
            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });

            app.Use((context, next) =>
            {
                var cultureQuery = context.Request.Query["culture"];
                if (!string.IsNullOrWhiteSpace(cultureQuery))
                {
                    var culture = new CultureInfo(cultureQuery);

                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }

                // Call the next delegate/middleware in the pipeline
                return next();
            });
            app.Map("/map1", HandleMapTest1);

            app.Map("/map2", HandleMapTest2);
            app.Map("/level1/level2", HandleMapTest2);
            app.MapWhen(context => context.Request.Query.ContainsKey("branch"),
                HandleBranch);
            //            app.Run(async context =>
            //            {
            //                await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
            //                await context.Response.WriteAsync(
            //                    $"Hello {CultureInfo.CurrentCulture.DisplayName}");
            //            });

            app.UseStaticFiles();

            //����Զ��徲̬��Դ·��
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = "/StaticFiles"
            });

            app.UseAuthentication();               // Authenticate before you access secure resources.

            app.UseMvc();
            app.UseMvcWithDefaultRoute();          // Add MVC to the request pipeline.

            //            app.Run((context) =>
            //            {
            //                context.Features.Get<IHttpMaxRequestBodySizeFeature>()
            //                    .MaxRequestBodySize = 10 * 1024;
            //                context.Features.Get<IHttpMinRequestBodyDataRateFeature>()
            //                    .MinDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
            //                context.Features.Get<IHttpMinResponseDataRateFeature>()
            //                    .MinDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
            //            };
        }

        #region ���� IApplicationBuilder.Map�����Ĵ����

        private static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }
        private static void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 2");
            });
        }

        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var branchVer = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Branch used = {branchVer}");
            });
        }

        #endregion

        public class AutofacModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<DiTest>().As<IDiTest>().PropertiesAutowired(); 
            }
        }

        public interface IDiTest
        {
        }

        public class DiTest : IDiTest
        {
            public int aa { get; set; } = 2;
        }
    }
}
