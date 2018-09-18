using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EFCore.Infrustructure;
using WebApplication3.DBContext;
using WebApplication3.Extension;
using WebApplication3.Kernel;

namespace WebApplication3
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //            var builder = new ConfigurationBuilder()
            //                .SetBasePath(env.ContentRootPath)
            //                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //                .AddJsonFile("CtsConfig/autofac.json")
            //                .AddEnvironmentVariables();
            //            configuration = builder.Build();
            GlobalSetting.SetConfiguration(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SchoolContext>(
                opt => opt.UseDataBaseBuilder(GlobalSetting.Configuration.GetConnectionString("MsSqlConnection"), "mssql"));
            services.AddScoped<DbContext>(provider => provider.GetService<SchoolContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            // Add controllers as services so they'll be resolved.
            services.AddMvc();//.AddControllersAsServices();

            // Create the autofac container builder.
            var builder = new ContainerBuilder();

            // When you do service population, it will include your controller types automatically.
            builder.Populate(services);
            builder.RegisterModule(new IocModule());
            //            var cc = GlobalSetting.GetConfigSection<WebSiteConfig>("website");
            //            builder.RegisterModule(new LoggingModule());
            //            var module = new ConfigurationModule(Configuration);
            //            builder.RegisterModule(module);
            var container = builder.Build();
            GlobalSetting.SetContainer(container);
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime, SchoolContext context)
        {
            //                        var ccc = GlobalSetting.AppContainer.Resolve<SchoolContext>();
            //                        var cc = GlobalSetting.AppContainer.Resolve<IUnitOfWork>();
            //                        var dd = cc as UnitOfWork<SchoolContext>;
            //                        var stdu = ccc.Students.FirstOrDefault();
            //                        stdu.LastName = stdu.LastName + "++";
            //            //            stdu.LastName = stdu.LastName.Substring(0, stdu.LastName.Length - 1);
            //            //            ccc.SaveChanges();
            //                        cc.BeginTransaction();
            //                        cc.SaveChanges();
            //                        cc.RollBackTransaction();
            //                        var fff = object.ReferenceEquals(ccc, dd.dbContext);


            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("default.html");
            app.UseDefaultFiles(options);

            app.UseStaticFiles();
            app.EnableCustomStaticFiles();
            app.EnableDirectoryBrowser();

            //            app.UseResponseCompression();
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            //            context.Database.EnsureCreated();

            appLifetime.ApplicationStopped.Register(Dispose);
        }

        private void Dispose()
        {
            GlobalSetting.AppContainer.Dispose();
        }
    }
}
