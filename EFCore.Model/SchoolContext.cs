using System;
using EFCore.Model.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCore.Model
{
    public class SchoolContext : DbContextBase<SchoolContext>
    {
        public override bool EnableDataBaseTracking { get; set; }
        public override Action<string> OnDatabaseTracking => DoDatabaseTracking;
        public override Func<string, LogLevel, bool> DatabaseTrackingFilter => Filter;

        public SchoolContext(DbContextOptions<SchoolContext> options, IServiceProvider provider)
            : base(options, provider)
        {
            EnableDataBaseTracking = false;
        }

        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>();
            modelBuilder.Entity<Enrollment>();
            modelBuilder.Entity<Student>();
        }

        private void DoDatabaseTracking(string contet)
        {
            //you can log here
            Console.Write(contet);
        }

        private bool Filter(string type, LogLevel level)
        {
            return type == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
        }
    }
}