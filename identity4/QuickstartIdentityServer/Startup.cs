using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace QuickstartIdentityServer
{
    public class Startup
    {
        //        public IConfiguration Configuration { get; }
        //        public Startup(IConfiguration configuration)
        //        {
        //            Configuration = configuration;
        //        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //            services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                //每次启动时，为令牌签名创建了一个临时密钥。在生成环境需要一个持久化的密钥。
                .AddDeveloperSigningCredential()
//                .AddTemporarySigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResourceResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddInMemoryPersistedGrants()
//                .AddAspNetIdentity<ApplicationUser>()
                //                .AddProfileService<ProfileService>()
                .AddTestUsers(Config.GetUsers())
//                // this adds the config data from DB (clients, resources)
//                .AddConfigurationStore(options =>
//                {
//                    options.ConfigureDbContext = builder =>
//                        builder.UseSqlServer(connectionString,
//                            sql => sql.MigrationsAssembly(migrationsAssembly));
//                })
//                // this adds the operational data from DB (codes, tokens, consents)
//                .AddOperationalStore(options =>
//                {
//                    options.ConfigureDbContext = builder =>
//                        builder.UseSqlServer(connectionString,
//                            sql => sql.MigrationsAssembly(migrationsAssembly));
//
//                    // this enables automatic token cleanup. this is optional.
//                    options.EnableTokenCleanup = true;
//                    options.TokenCleanupInterval = 30;
//                })
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }
}
