using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace XinDaYang.RentalCars.Tools.Pagenation
{
    public static class ScriptResourceExtensions
    {
        private static string GetMvcPagerScriptUrl(HttpContextBase context)
        {
            var page = context.CurrentHandler as Page;
            return (page ?? new Page()).ClientScript.GetWebResourceUrl(typeof(PagerExtensions), "XinDaYang.RentalCars.Tools.Pagenation.MvcPager.min.js");
        }

        
        public static void RegisterMvcPagerScriptResource(this HtmlHelper html)
        {
#if DEBUG
            html.ViewContext.Writer.Write("<script type=\"text/javascript\" src=\"/scripts/MvcPager.js\"></script>");
#else
            html.ViewContext.Writer.Write("<script type=\"text/javascript\" src=\"" + GetMvcPagerScriptUrl(html.ViewContext.HttpContext) + "\"></script>");
#endif
        }

        
        public static void LoadMvcPagerScript(this AjaxHelper ajax)
        {
            ajax.ViewContext.Writer.Write("if(!$.fn.initMvcPagers){$.getScript(\""+GetMvcPagerScriptUrl(ajax.ViewContext.HttpContext)+"\");}");
        }

    }
}
