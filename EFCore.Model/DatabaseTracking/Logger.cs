using System;
using Microsoft.Extensions.Logging;

namespace EFCore.Model.DatabaseTracking
{
    public class Logger : ILogger
    {
        private readonly string categoryName;
        private readonly LogProvider provider;
        public Logger(string categoryName, LogProvider provider)
        {
            this.provider = provider;
            this.categoryName = categoryName;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //grab a reference to the current logger settings for consistency, 
            //and to eliminate the need to block a thread reconfiguring the logger
            var config = provider.Configuration;
            if (config.Filter(categoryName, logLevel))
            {
                var content = formatter(state, exception);
                config.Logger(content);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}