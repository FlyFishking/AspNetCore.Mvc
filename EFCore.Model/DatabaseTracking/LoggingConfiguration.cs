using System;
using Microsoft.Extensions.Logging;

namespace EFCore.Model.DatabaseTracking
{
    public class LoggingConfiguration
    {
        public LoggingConfiguration(Action<string> logger, Func<string, LogLevel, bool> filter)
        {
            this.Logger = logger;
            this.Filter = filter;
        }
        public readonly Action<string> Logger;
        public readonly Func<string, LogLevel, bool> Filter;
    }
}