using EFCore.Kernal;
using EFCore.Kernal.ProviderModule;
using EFCore.Model;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EFCore.Infrustructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WebApplication2.Kernel
{
    public static class ServiceCollectionExtensions
    {
        public static void UseConfig(this IConfiguration configuration)
        {
            //            var builder = new ConfigurationBuilder()
            //                .SetBasePath(env.ContentRootPath)
            //                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //                .AddJsonFile("autofac.json")
            //                .AddEnvironmentVariables();
            //            configuration = builder.Build();
            GlobalSetting.SetConfiguration(configuration);
        }

        /// <summary>
        /// add database context
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddDbContext<SchoolContext>(
                opt => opt.UseDataBaseBuilder(GlobalSetting.Configuration.GetConnectionString("MySqlConnection"), "mysql"));
            services.AddScoped<DbContext>(provider => provider.GetService<SchoolContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddSessionCache(this IServiceCollection services, bool useRedis = false)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (useRedis)
            {
                services.AddDistributedRedisCache(option =>
                    {
                        option.Configuration = GlobalSetting.Configuration.GetConnectionString("RedisConnection");
                        option.InstanceName = "master";
                    }
                );
            }
            else
            {
                //                services.AddMemoryCache();
            }
            services.AddSession();
            return services;
        }

        //for more detail see https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-2.1#set-the-allowed-origins
        public static IServiceCollection AddWebsiteCrose(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

//            //global cors
//            services.Configure<MvcOptions>(options =>
//            {
//                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
//            });

            services.AddCors(opt =>
                {
                    opt.AddPolicy("AllowSpecificOriginAndAllMethodAllHeader",
                        builder => builder.WithOrigins("http://localhost:8888", "http://localhost:8899")
                            .AllowAnyMethod().AllowAnyHeader());
//                    opt.AddPolicy("AllowSpecificOrigin",
//                        builder => builder.WithOrigins("http://localhost:8888", "http://localhost:8899")
//                            .AllowAnyMethod().WithExposedHeaders("x-custom-header"));
//                    opt.AddPolicy("SetPreflightExpiration",
//                        builder => builder.WithOrigins("http://localhost:8888")
//                                .SetPreflightMaxAge(TimeSpan.FromSeconds(2520)));
                });

            return services;
        }

        //for more details see https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/using-data-protection?view=aspnetcore-2.1
        public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddDataProtection()
                .SetApplicationName("website")

                //windows dpaip 作为主加密键
                .ProtectKeysWithDpapi()
                //                //如果是 windows 8+ 或者windows server2012+ 可以使用此选项（基于Windows DPAPI-NG）
                //                .ProtectKeysWithDpapiNG("SID={current account SID}", DpapiNGProtectionDescriptorFlags.None)
                //                //如果是 windows 8+ 或者windows server2012+ 可以使用此选项（基于证书）
                //                .ProtectKeysWithDpapiNG("CERTIFICATE=HashId:3BCE558E2AD3E0E34A7743EAB5AEA2A9BD2575A0", DpapiNGProtectionDescriptorFlags.None)
                //                //使用证书作为主加密键，目前只有widnows支持，linux还不支持。
                //                .ProtectKeysWithCertificate("thumbprint")
                //call the specify encrypt 
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                })

                // lift time of the key, default is 90 days
                .SetDefaultKeyLifetime(TimeSpan.FromDays(14))

                //store the private key to a public single server for multy access（windows、Linux、macOS）
                .PersistKeysToFileSystem(new DirectoryInfo(@"\\server\share\directory\"))
                //                //only suite for windows registry
                //                .PersistKeysToRegistry(Microsoft.Win32.RegistryKey.FromHandle(null))

                //disable auto generate private key file, to use public single ke file via PersistKeysToFileSystem specify
                .DisableAutomaticKeyGeneration();

            //below tow lines 
            //            services.AddDataProtection(x => x.ApplicationDiscriminator = "my_app_sample_identity");
            //            services.AddDataProtection().SetApplicationName("my_app_sample_identity");
            return services;
        }

        /// <summary>
        /// use autofac
        /// call it at <see cref="Startup.ConfigureServices">Config</see> method by 
        ///             return services.AddProvider();
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider AddProvider(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var svcProvider = ProviderManager.Instance.Initialize(services);
            GlobalSetting.SetContainer(ProviderManager.Instance.Container);
            return svcProvider;
        }
    }

    public class TestProviderOptions : IOptions<TestProviderOptions>
    {
        public int DefaultLevel { get; set; } = 1;
        public TestProviderOptions Value => this;
    }

    public class RedisXmlRepository : IXmlRepository, IDisposable
    {
        public static readonly string RedisHashKey = "DataProtectionXmlRepository";

        private IConnectionMultiplexer connection;

        private bool disposed = false;

        public RedisXmlRepository(string connectionString, ILogger<RedisXmlRepository> logger)
            : this(ConnectionMultiplexer.Connect(connectionString), logger)
        {
        }

        public RedisXmlRepository(IConnectionMultiplexer connection, ILogger<RedisXmlRepository> logger)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            this.connection = connection;
            this.Logger = logger;

            var configuration = Regex.Replace(this.connection.Configuration, @"password\s*=\s*[^,]*", "password=****", RegexOptions.IgnoreCase);
            this.Logger.LogDebug("Storing data protection keys in Redis: {RedisConfiguration}", configuration);
        }

        public ILogger<RedisXmlRepository> Logger { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
        }
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var database = this.connection.GetDatabase();
            var hash = database.HashGetAll(RedisHashKey);
            var elements = new List<XElement>();

            if (hash == null || hash.Length == 0)
            {
                return elements.AsReadOnly();
            }

            foreach (var item in hash.ToStringDictionary())
            {
                elements.Add(XElement.Parse(item.Value));
            }

            this.Logger.LogDebug("Read {XmlElementCount} XML elements from Redis.", elements.Count);
            return elements.AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (string.IsNullOrEmpty(friendlyName))
            {
                friendlyName = Guid.NewGuid().ToString();
            }

            this.Logger.LogDebug("Storing XML element with friendly name {XmlElementFriendlyName}.", friendlyName);

            this.connection.GetDatabase().HashSet(RedisHashKey, friendlyName, element.ToString());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.connection != null)
                    {
                        this.connection.Close();
                        this.connection.Dispose();
                    }
                }

                this.connection = null;
                this.disposed = true;
            }
        }
    }
}