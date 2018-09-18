using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication3.DBContext.DataBaseTracking;
using WebApplication3.DBContext.Models;

namespace WebApplication3.DBContext
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");

            base.OnModelCreating(modelBuilder);
        }


#if DEBUG
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new DbTracingLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }
#endif
    }
}