using System.Threading.Tasks;
using EFCore.Kernal.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFCore.Kernal.Filter
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            JsonResult result = null;
            if (exception is BusinessException)
            {
                result = new JsonResult(exception.Message)
                {
                    StatusCode = exception.HResult
                };
            }
            else
            {
                result = new JsonResult("服务器处理出错")
                {
                    StatusCode = 500
                };
                var logger = GlobalSetting.GetLog4Net<CustomExceptionAttribute>();
                logger.Error("服务器处理出错");
            }
            context.Result = result;
        }
    }
}