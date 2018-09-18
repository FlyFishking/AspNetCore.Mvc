using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace EFCore.Kernal.Filter
{
    public class AddHeaderAttribute : ResultFilterAttribute
    {
        private readonly string name;
        private readonly string value;
        public AddHeaderAttribute(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (string.IsNullOrEmpty(name))
            {
                context.HttpContext.Response.Headers.Add(name, new StringValues(value));
            }
            base.OnResultExecuting(context);
        }
    }

    public class AddHeaderWithFactoryAttribute : Attribute, IActionFilter
    {
        public AddHeaderWithFactoryAttribute()
        {

        }
        // Implement IFilterFactory
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new InternalAddHeaderFilter();
        }

        private class InternalAddHeaderFilter : IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext context)
            {
                context.HttpContext.Response.Headers.Add(
                    "Internal", new string[] { "Header Added" });
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
            }
        }
        public bool IsReusable => false;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var cc = context;
            var log = GlobalSetting.GetLog4Net<AddHeaderWithFactoryAttribute>();
            log.Info("1111111111111111111111");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var cc = context;
            var log = GlobalSetting.GetLog4Net<AddHeaderWithFactoryAttribute>();
            log.Info("2222222222222");
        }
    }
}