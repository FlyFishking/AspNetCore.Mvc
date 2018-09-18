using Microsoft.Extensions.Logging;
using System;

namespace EFCore.Model.DataBaseTracking
{
    /// <summary>
    /// Track the sql and log sql query to local file(.txt)
    /// <example>
    /// <![CDATA[
    ///         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    ///         {
    ///             var loggerFactory = new LoggerFactory();
    ///             loggerFactory.AddProvider(new DbTracingLoggerProvider());
    ///             optionsBuilder.UseLoggerFactory(loggerFactory);
    ///             base.OnConfiguring(optionsBuilder);
    ///         }
    /// ]]>
    /// </example>
    /// </summary>
    public class DbTracingLogger : ILogger
    {
        private readonly string categoryName;
        public DbTracingLogger(string categoryName) => this.categoryName = categoryName;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command")
            {
                if (logLevel == LogLevel.Information)
                {
                    var logContent = formatter(state, exception);
//                    GlobalSetting.GetLog4Net(categoryName).Debug(logContent);
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}