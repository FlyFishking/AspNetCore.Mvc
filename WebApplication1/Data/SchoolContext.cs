using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
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
        }
    }
    /// <summary>
    /// MySQL的数据库配置
    /// </summary>
    public class MySQLDataBaseConfig
    {
        /// <summary>
        /// 默认的MySQL的链接字符串
        /// </summary>
        private const string DefaultMySqlConnectionString = "server=192.168.30.111;userid=root;pwd=root;port=3306;database=univ;";
        public static SchoolContext CreateContext(string mySqlConnectionString = null)
        {
            var conn = $"server={"192.168.20.64"};user id={"root"};password={"root"};persistsecurityinfo=True;port={3306};database={"univ"};SslMode=none";
            var context = DBContextHelper.CreateContext<SchoolContext>(conn, (opt) => new SchoolContext(opt), "MySql");
            DbInitializer.Initialize(context);
            return context;
        }

    }
}