using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFCore.Kernal.Filter
{
    public class StopwatchAttribute : ActionFilterAttribute
    {
        private Stopwatch sw;
        private readonly string name;

        public StopwatchAttribute(string viewDataName = "ElapsedTime")
        {
            name = viewDataName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            sw = new Stopwatch();
            sw.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            sw.Stop();
            var elapsed = sw.Elapsed;
            var elapse = (double)elapsed.Seconds;
            var type = filterContext.Controller.GetType();
//                       ((Microsoft.AspNetCore.Mvc) filterContext.Controller).ViewData[name] = (object)elapse.ToString((IFormatProvider)CultureInfo.InvariantCulture);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}