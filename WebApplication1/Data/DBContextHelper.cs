using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1.Data
{
    public class DBContextHelper
    {
        public static T CreateContext<T>(string connString, Func<DbContextOptions<T>, T> fun, string type = "mysql")
            where T : DbContext
        {
            if (string.IsNullOrEmpty(connString))
            {
                throw new ArgumentException("param conn should not be null!");
            }
            if (fun == null)
            {
                throw new ArgumentException("param fun should not be null!");
            }
            var opt = new DbContextOptionsBuilder<T>();
            switch (type.ToUpper())
            {
                case "MYSQL":
                    //Install-Package MySql.Data.EntityFrameworkCore -Pre
                    opt.UseMySQL(connString);
                    break;
                case "MSSQL":
                    opt.UseSqlServer(connString);
                    break;
                default:
                    break;
            }
            var context = fun(opt.Options);
            context.Database.EnsureCreated();
            return context;
        }

        public static void DbContextDone<T>(IWebHost host, Action<T> action) where T : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<T>();
                    action?.Invoke(context);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while seeding the database.");
                }
            }
        }
    }
}