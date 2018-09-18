using Autofac;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using WebApplication3.CtsUtils.JsonSettingConfig;

namespace WebApplication3.Kernel
{
    public class GlobalSetting
    {
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// <example>
        /// <![CDATA[
        /// var cc = GlobalSetting.AppContainer.Resolve<SchoolContext>();
        /// ]]>
        /// </example>
        /// </summary>
        public static IContainer AppContainer { get; private set; }

        /// <summary>
        /// <example>
        /// <![CDATA[
        /// GlobalSetting.SetContainer(container);
        /// using (var scope = GlobalSetting.Provider.CreateScope())
        /// {
        ///     var p = scope.ServiceProvider;
        ///     var ccc = p.GetService<IDiTest>();
        ///     var aaa = p.GetService<SchoolContext>();
        /// }
        /// ]]>
        /// </example>
        /// </summary>
        private static IServiceProvider Provider { get; set; }

        private static ILoggerRepository LoggerRepository { get; set; }

        public static void SetConfiguration(IConfiguration config)
        {
            Configuration = config;
        }

        public static IConfiguration GetConfiguration(string configFilePath, string configFileName = "appsettings.json")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(configFilePath)
                .AddJsonFile(configFileName, false, true);
            return builder.Build();
        }

        public static void SetContainer(IContainer container)
        {
            if (AppContainer == null)
            {
                AppContainer = container;
            }
        }

        public static void SetProvider(IServiceProvider provider)
        {
            Provider = provider;
        }

        //        public static void SetLoggerRepo(ILoggerRepository logger)
        //        {
        //            LoggerRepository = logger;
        //        }

        public static T GetConfigSection<T>(string key) where T : class
        {
            return Configuration.GetSection(key).Get<T>();
        }

        public static IConfiguration GetConfigSection(string key)
        {
            return Configuration.GetSection(key);
        }

        public static string GetConfigSectionValue(string key)
        {
            return Configuration.GetSection(key).Value;
        }

        public static ILog GetLog4Net(string name)
        {
            LoadDefaultLogger();
            return GetLog4Net(LoggerRepository.Name, name);
        }

        public static ILog GetLog4Net(string repository, string name)
        {
            return LogManager.GetLogger(repository, name);
        }

        public static ILog GetLog4Net<T>() where T : class
        {
            LoadDefaultLogger();
            return GetLog4Net<T>(LoggerRepository.Name);
        }

        public static ILog GetLog4Net<T>(string repository) where T : class
        {
            return LogManager.GetLogger(repository, typeof(T));
        }

        private static void LoadDefaultLogger()
        {
            if (LoggerRepository != null) return;
            var webSiteSetting = GetConfigSection<WebSiteConfig>("WebSite");
            var logger = LogManager.CreateRepository(webSiteSetting.DefaultRepoName);
            XmlConfigurator.Configure(logger, new FileInfo(webSiteSetting.Log4NetFile));
            LoggerRepository = logger;
        }
    }
}