using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Model
{
    public static class DbContextExtension
    {
        /// <summary>
        /// Get dbcontext by using specify connection string
        /// <![CDATA[
        /// e.g.
        /// var dbType = Configuration.GetSection("DataBaseType").Value;
        /// DBContextHelper.CreateDBContext&lt;SchoolContext&gt;(Configuration.GetConnectionString(dbType + "Connection"), (opt) => new SchoolContext(opt), dbType);
        /// ]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connString"></param>
        /// <param name="fun"></param>
        /// <param name="type">database type. e.g. mssql:microsoft sql server</param>
        /// <param name="doEnsureCreated">if true will call the context.Database.EnsureCreated() function</param>
        /// <returns></returns>
        public static T CreateDbContext<T>(string connString, Func<DbContextOptions<T>, T> fun, string type = "mysql", bool doEnsureCreated = false)
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
                    opt.UseSqlServer(connString);
                    break;
            }
            var context = fun(opt.Options);
            if (doEnsureCreated)
            {
                context.Database.EnsureCreated();
            }
            return context;
        }

        public static DbContextOptionsBuilder UseDataBaseBuilder(this DbContextOptionsBuilder opt, string connString, string type = "mysql")
        {
            switch (type.ToUpper())
            {
                case "MYSQL":
                    //Install-Package MySql.Data.EntityFrameworkCore -Pre
                    opt.UseMySQL(connString);
                    break;
                case "MSSQL":
                    //                    opt.UseSqlServer(connString, t => t.UseRowNumberForPaging());
                    opt.UseSqlServer(connString);
                    break;
                default:
                    opt.UseSqlServer(connString);
                    break;
            }
            return opt;
        }

        public static bool EnsureCreatedDataBase<TContext, TEntity>(this DbContextBase<TContext> context)
            where TContext : DbContext
            where TEntity : class
        {
            context.Database.EnsureCreated();
            //            context.Database.Migrate();
            var result = context.Set<TEntity>().Any();
            return result;
        }


        //        /// <summary>
        //        /// just for test
        //        /// <remarks>
        //        /// <![CDATA[
        //        /// e.g. 
        //        /// DbContextHelper.DbContextDone<SchoolContext>(host, DbInitializer.Initialize);
        //        /// ]]>
        //        /// </remarks>
        //        /// </summary>
        //        /// <typeparam name="T"></typeparam>
        //        /// <param name="host"></param>
        //        /// <param name="action"></param>
        //        public static void DbContextDone<T>(IWebHost host, Action<T> action)
        //            where T : DbContext
        //        {
        //            using (var scope = host.Services.CreateScope())
        //            {
        //                var services = scope.ServiceProvider;
        //                try
        //                {
        //                    var context = services.GetRequiredService<T>();
        //                    action?.Invoke(context);
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new Exception("An error occurred while seeding the database.");
        //                }
        //            }
        //        }
    }
}