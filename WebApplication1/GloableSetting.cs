using Autofac;
using log4net.Repository;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    public class GloableSetting
    {
        public static  IConfiguration Configuration { get; private set; }
        public static IContainer AppContainer { get; private set; }
        public static ILoggerRepository LoggerRepository { get; set; }

        public GloableSetting(IConfiguration config)
        {
            Configuration = config;
        }

        public void SetContainer(IContainer container)
        {
            AppContainer = container;
        }

        public void SetLoggerRepo(ILoggerRepository logger)
        {
            LoggerRepository = logger;
        }
    }
}