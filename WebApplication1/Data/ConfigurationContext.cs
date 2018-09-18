using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ConfigurationValue> Values { get; set; }
    }
}