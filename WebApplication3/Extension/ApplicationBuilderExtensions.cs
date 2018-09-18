using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using WebApplication3.Kernel;

namespace WebApplication3.Extension
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnableCustomStaticFiles(this IApplicationBuilder app, string key = "StaticFile")
        {
            var staticFile = GlobalSetting.GetConfigSection<Dictionary<string, string>>(key);
            if (staticFile != null)
            {
                var provider = new FileExtensionContentTypeProvider();
                provider.Mappings[".rtf"] = "application/x-msdownload";
                provider.Mappings.Remove(".mp4");
                foreach (var file in staticFile)
                {
                    if (string.IsNullOrEmpty(file.Key) || string.IsNullOrEmpty(file.Value)) continue;
                    var reqPath = file.Value.StartsWith("/") ? file.Value : $"/{file.Value}";
                    //                    provider.Mappings.Add(file.Key, reqPath);
                    //                    var fileServerOpt = new FileServerOptions() { EnableDirectoryBrowsing = true };
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), file.Key)),
                        RequestPath = new PathString(reqPath),
//                        ContentTypeProvider = provider
                    });
                }
                //                app.UseStaticFiles(new StaticFileOptions() { ContentTypeProvider = provider });
            }
            return app;
        }

        public static IApplicationBuilder EnableDirectoryBrowser(this IApplicationBuilder app, string key = "WebBrowserFolder")
        {
            var browserList = GlobalSetting.GetConfigSection<Dictionary<string, string>>(key);
            if (browserList != null)
            {
                foreach (var browser in browserList)
                {
                    if (string.IsNullOrEmpty(browser.Key) || string.IsNullOrEmpty(browser.Value)) continue;
                    var reqPath = browser.Value.StartsWith("/") ? browser.Value : $"/{browser.Value}";
                    app.UseDirectoryBrowser(new DirectoryBrowserOptions()
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), browser.Key)),
                        RequestPath = new PathString(reqPath)
                    });
                }
            }
            return app;
        }
    }
}