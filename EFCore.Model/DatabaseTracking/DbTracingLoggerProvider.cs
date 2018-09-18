using System;
using Microsoft.Extensions.Logging;

namespace EFCore.Model.DataBaseTracking
{
    public class DbTracingLoggerProvider : ILoggerProvider
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ILogger CreateLogger(string categoryName) => new DbTracingLogger(categoryName);
    }
}