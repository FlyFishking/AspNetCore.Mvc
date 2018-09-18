using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFCore.Kernal.Filter
{
    public class SampleActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var aa = "32";
            // do something before the action executes
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var aa = "done";
            // do something after the action executes
        }
    }

    public class SampleAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var aa = "";
            // do something before the action executes
            var resultContext = await next();
            aa = "22";
            // do something after the action executes; resultContext.Result will be set
        }
    }
}