using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = IdentityClient.MainAsync();
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository1");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            ILog log = LogManager.GetLogger(repository.Name, typeof(Program));

            log.Info("NETCorelog4net log");
            log.Info("test log");
            log.Error("error");
            log.Info("linezero");
            log.Error("12321",new ArgumentException("params error"));
            Console.ReadKey();

//            DisplayCurrentInfo().Wait();
            var cc = TransExpress<a, b>.Clone(new a { age = 1, c = 2, dd = 3.0f, name = "fds", time = DateTime.Now });
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
        static async Task DisplayCurrentInfo()
        {
            Task wait = WaitAndApologize();

            string output = $"Today is {DateTime.Now:D}\n" +
                            $"The current time is {DateTime.Now.TimeOfDay:t}\n" +
                            $"The current temperature is 76 degrees.\n";
            await wait;
            Console.WriteLine(output);
            //
            //            await WaitAndApologize();
            //            Console.WriteLine($"Today is {DateTime.Now:D}");
            //            Console.WriteLine($"The current time is {DateTime.Now.TimeOfDay:t}");
            //            Console.WriteLine("The current temperature is 76 degrees.");
        }

        static async Task WaitAndApologize()
        {
            // Task.Delay is a placeholder for actual work.  
            await Task.Delay(2000);
            // Task.Delay delays the following line by two seconds.  
            Console.WriteLine("\nSorry for the delay. . . .\n");
        }
    }
}
public class a
{
    public string aaaaa { get; }
    private string aaaa { get; set; }
    public float dd { get; set; }
    public string name { get; set; }
    public int age { get; set; }
    public double c { get; set; }
    public DateTime? time { get; set; }
}

public class b
{
    private string ee { get; set; }
    public int name { get; set; }
    public int age { get; set; }
    public double c { get; set; }
    public DateTime? time { get; set; }
    public string dd { get; set; }
}

/// <summary>
/// 将两个对象中相同字段的值拷贝到目标<see cref="TOut"/>对象
/// </summary>
/// <typeparam name="TIn">源对象</typeparam>
/// <typeparam name="TOut">目标对象</typeparam>
public static class TransExpress<TIn, TOut>
{
    private static readonly Func<TIn, TOut> cache = GetFunc();

    private static Func<TIn, TOut> GetFunc()
    {
        var typeIn = typeof(TIn);
        var expressParameter = Expression.Parameter(typeIn, typeIn.FullName);

        var memberBindingList = new List<MemberBinding>();
        foreach (var item in typeof(TOut).GetProperties())
        {
            if (item.CanWrite)
            {
                var tInProperty = typeof(TIn).GetProperty(item.Name);
                if (tInProperty != null && tInProperty.CanRead && tInProperty.PropertyType.FullName == item.PropertyType.FullName)
                {
                    var property = Expression.Property(expressParameter, tInProperty);
                    var memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }
            }
        }

        var memberInitExpress = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
        //        var memberInitExpress = Expression.MemberInit(Expression.New(typeof(TOut)), (from item in typeof (TOut).GetProperties() where item.CanWrite let tInProperty = typeof (TIn).GetProperty(item.Name) where tInProperty != null && tInProperty.CanRead && tInProperty.PropertyType.FullName == item.PropertyType.FullName let property = Expression.Property(expressParameter, tInProperty) select Expression.Bind(item, property)).Cast<MemberBinding>().ToArray());

        var lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpress, expressParameter);
        return lambda.Compile();
    }

    /// <summary>
    /// 将两个对象中相同字段的值拷贝到目标对象
    /// <example>
    /// <![CDATA[var cc = TransExpress<a, b>.Clone(new a { age = 1, c = 2, dd = 3.0f, name = "fds", time = DateTime.Now });]]>
    /// </example>
    /// </summary>
    /// <param name="tin">源对象</param>
    /// <returns>目标对象</returns>
    public static TOut Clone(TIn tin)
    {
        return cache(tin);
    }
}

public static class IEnumertor
{
}