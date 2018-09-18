using EFCore.Kernal;
using EFCore.Kernal.Common;
using EFCore.Kernal.Filter;
using EFCore.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.WebCore.Extention;
using System;
using System.Linq;
using EFCore.Model.Model;
using Microsoft.EFCore.Infrustructure;

namespace WebApplication2.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public static void StaticPublic() { }
        public static void StaticPrivate() { }
        public static string StaticStringFields;
        public static string StaticStringProp { get; set; }
        private readonly IStudentService svcStudent;
        private readonly IHttpContextAccessor httpAccessor;
        private IValuesService svcValue { get; set; }

        private readonly IRepository<Student> repoStu;

        private IMemoryCache memoryCache;
        public IndexModel(IStudentService student,
            IValuesService value,
            IServiceProvider provider,
            IHttpContextAccessor accessor,
            IMemoryCache cache,
            IOptions<WebSiteConfig> config,
            IRepository<Student> stu)
        {
            repoStu = stu;
            accessor.HttpContext.Session.SetString("12", "321321");
            var sessionTest = accessor.HttpContext.Session.GetString("12");
            sessionTest = accessor.HttpContext.Session.GetString("123");
            //            var cc = (SchoolContext)provider.GetService(typeof(SchoolContext));
            //            cc.ConfigureLogging(t => Console.Write(t), LoggingCategories.SQL);
            //            var sc = GlobalSetting.Resove<SchoolContext>();

            svcStudent = student;
            httpAccessor = accessor;
            //            var cc =value.FindAll();
            //            svcValue = value;
            this.memoryCache = cache;
            var cc = AssemblerExtension.GetAppDomainAssemblies();
            var typeCtrl = svcStudent.GetType();
            var typeService = typeof(IndexModel);
            var assemTest = typeService.GetProperty("StaticStringProp", true);
            var ccc = typeService.GetInstanceMethod("OnGet");
            var cccc = typeService.GetInstanceFieldsAndProperties(false);
            var aaa = typeService.FindClassesOfInterfaceType();
            var assem1 = typeCtrl.FullName;
            var lt = BitConverter.GetBytes(23332);
            var isLt = BitConverter.IsLittleEndian;
            var btccc = lt.Reverse().ToArray();
            var bt = CommonUtil.ConvertToBigEndianOrder(lt);
            var stcc = bt.Select(t => t.ToString("X2")).JoinString("");
        }

//        [AddHeader("Author", "Rick")]
        [EnableCors("CorsSample")]
        //        [TestAsyncActionFilter]
        [AddHeaderWithFactory]
        //        [UseStopwatch]
        public void OnGet()
        {
            //            throw new Exception("afda");
            GetOrCreate();
            CreateCallbackEntry();
            var log = GlobalSetting.GetLog4Net<IndexModel>();
            var cc = svcStudent.GetQuery().ToList();
            var abc = svcStudent.Get(1);
            log.Info($"count:{cc.Count},first:{abc.LastName}");
            abc.LastName = "999";
            svcStudent.Update(abc);
        }

        public void TryGet()
        {
            DateTime cacheEntry;
            if (!memoryCache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                cacheEntry = DateTime.Now;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                memoryCache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }
        }

        public void GetOrCreate()
        {
            var cc = memoryCache.Get(CacheKeys.Entry);
            memoryCache.GetOrCreate(CacheKeys.Entry, c =>
            {
                c.SlidingExpiration = TimeSpan.FromSeconds(10);
                return DateTime.Now;
            });
        }

        public void CreateCallbackEntry()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Pin to cache.
                .SetPriority(CacheItemPriority.NeverRemove)
                // Add eviction callback
                .RegisterPostEvictionCallback(callback: EvictionCallback, state: this);

            memoryCache.Set(CacheKeys.CallbackEntry, DateTime.Now, cacheEntryOptions);
        }

        public void RemoveCallbackEntry()
        {
            memoryCache.Remove(CacheKeys.CallbackEntry);
        }

        private static void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            var message = $"Entry was evicted. Reason: {reason}.";
            ((IndexModel)state).memoryCache.Set(CacheKeys.CallbackMessage, message);
        }
    }
    public static class CacheKeys
    {
        public static string Entry { get { return "_Entry"; } }
        public static string CallbackEntry { get { return "_Callback"; } }
        public static string CallbackMessage { get { return "_CallbackMessage"; } }
        public static string Parent { get { return "_Parent"; } }
        public static string Child { get { return "_Child"; } }
        public static string DependentMessage { get { return "_DependentMessage"; } }
        public static string DependentCTS { get { return "_DependentCTS"; } }
        public static string Ticks { get { return "_Ticks"; } }
        public static string CancelMsg { get { return "_CancelMsg"; } }
        public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
    }
}
