using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace EFCore.Model.DatabaseTracking
{
    public class LogProvider : ILoggerProvider
    {
        //volatile to allow the configuration to be switched without locking
        public volatile LoggingConfiguration Configuration;
        private static bool DefaultFilter(string categoryName, LogLevel level) => true;
        private static readonly ConcurrentDictionary<Type, LogProvider> providers = new ConcurrentDictionary<Type, LogProvider>();

        private LogProvider(Action<string> logger, Func<string, LogLevel, bool> filter)
        {
            this.Configuration = new LoggingConfiguration(logger, filter);
        }

        public static void CreateOrModifyLoggerForDbContext(Type dbContextType,
            ILoggerFactory loggerFactory,
            Action<string> logger,
            Func<string, LogLevel, bool> filter = null)
        {
            var isNew = false;
            var provider = providers.GetOrAdd(dbContextType, t =>
            {
                var p = new LogProvider(logger, filter ?? DefaultFilter);
                loggerFactory.AddProvider(p);
                isNew = true;
                return p;
            });
            if (!isNew)
            {
                provider.Configuration = new LoggingConfiguration(logger, filter ?? DefaultFilter);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName, this);
        }

        public void Dispose() { }
    }
}