using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication1.Data;

namespace WebApplication1
{
    public class Program
    {
        public static ILoggerFactory loggerFactory;
        public static void Main(string[] args)
        {
            //            BuildWebHost(args).Run();
            var host = BuildWebHost(args);
            DBContextHelper.DbContextDone<SchoolContext>(host, DbInitializer.Initialize);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()

                //配置 Kestrel 选项
                .UseKestrel(options =>
                {
                    //可使用以下代码为整个应用程序设置并发打开 TCP 的最大连接数：
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                    //为整个应用程序和每个请求配置约束
                    //也可以用属性 的方式设置单个请求的正文大小,默认的请求正文最大大小为 30,000,000 字节，大约 28.6MB。
                    //[RequestSizeLimit(100000000)]
                    //public IActionResult MyActionMethod()
                    options.Limits.MaxRequestBodySize = 10 * 1024;

                    /*
                     * Kestrel 每秒检查一次数据是否以指定的速率（字节/秒）进入。 
                     * 如果速率低于最小值，则连接超时。宽限期是 Kestrel 提供给客户端用于将其发送速率提升到最小值的时间量；
                     * 在此期间不会检查速率。 宽限期有助于避免最初由于 TCP 慢启动而以较慢速率发送数据的连接中断。
                       默认的最小速率为 240 字节/秒，包含 5 秒的宽限期。
                       最小速率也适用于响应。 除了属性和接口名称中具有 RequestBody 或 Response 以外，
                       用于设置请求限制和响应限制的代码相同。
                     */
                    //以下示例演示如何在 Program.cs 中配置最小数据速率：
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Listen(IPAddress.Loopback, 5000);
                    //                    options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                    //                    {
                    //                        listenOptions.UseHttps("testCert.pfx", "testPassword");
                    //                    });
                })
            .ConfigureLogging(f =>
                {
                    f.AddConsole(t => { t.IncludeScopes = true; });
                })
                //
                //                                .ConfigureLogging((hostingcontext, logging) =>
                //                                {
                //                                    logging.AddConsole();
                //                                    logging.AddDebug();
                //                                })
                .Build();
    }
}
