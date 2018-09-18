using System;
using EFCore.Model.DatabaseTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCore.Model
{
    public abstract class DbContextBase<TContext> : DbContext
        where TContext : DbContext
    {
        private readonly IServiceProvider provider;
        public abstract bool EnableDataBaseTracking { get; set; }
        public abstract Action<string> OnDatabaseTracking { get; }
        public abstract Func<string, LogLevel, bool> DatabaseTrackingFilter { get; }

        protected DbContextBase(DbContextOptions options, IServiceProvider provider)
            : base(options)
        {
            this.provider = provider;
        }

#if DEBUG
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (EnableDataBaseTracking)
            {
                var loggerFactory = (ILoggerFactory)provider.GetService(typeof(ILoggerFactory));
                LogProvider.CreateOrModifyLoggerForDbContext(typeof(TContext),
                    loggerFactory,
                    OnDatabaseTracking,
                    DatabaseTrackingFilter);
                optionsBuilder.UseLoggerFactory(loggerFactory);
                base.OnConfiguring(optionsBuilder);
            }
        }
#endif
    }
}