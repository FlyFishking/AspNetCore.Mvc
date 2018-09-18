using System;
using System.IO;
using System.Web.Mvc;

[assembly: System.Web.UI.WebResource("XinDaYang.RentalCars.Tools.Pagenation.MvcPager.min.js", "text/javascript")]
namespace XinDaYang.RentalCars.Tools.Pagenation
{
    
    public static class PagerExtensions
    {
        #region Html Pager

        
        public static HtmlPager Pager(this HtmlHelper helper, int totalItemCount, int pageSize, int pageIndex, PagerOptions pagerOptions)
        {
            return new HtmlPager(helper,totalItemCount,pageSize,pageIndex,pagerOptions);
        }

        
        public static HtmlPager Pager(this HtmlHelper helper, int totalItemCount, int pageSize, int pageIndex)
        {
            return new HtmlPager(helper,totalItemCount, pageSize,pageIndex);
        }

        
        public static HtmlPager Pager(this HtmlHelper helper, IPagedList pagedList)
        {
            if (pagedList == null)
            {
                throw new ArgumentNullException("pagedList");
            }
            return new HtmlPager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null);
        }

        
        public static HtmlPager Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions)
        {
            if (pagedList == null)
            {
                throw new ArgumentNullException("pagedList");
            }
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, pagerOptions);
        }


        #endregion
        
        #region Ajax Pager

        
        public static AjaxPager Pager(this AjaxHelper ajax, int totalItemCount, int pageSize, int pageIndex, PagerOptions pagerOptions, MvcAjaxOptions ajaxOptions)
        {
            return new AjaxPager(ajax, totalItemCount, pageSize, pageIndex, pagerOptions, ajaxOptions);
        }

        
        public static AjaxPager Pager(this AjaxHelper ajax, IPagedList pagedList)
        {
            return new AjaxPager(ajax, pagedList, null,null);
        }

        
        public static AjaxPager Pager(this AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions)
        {
            return Pager(ajax, pagedList, pagerOptions, null);
        }

        
        public static AjaxPager Pager(this AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions, MvcAjaxOptions ajaxOptions)
        {
            if (pagedList == null)
            {
                throw new ArgumentNullException("pagedList");
            }
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex,pagerOptions, ajaxOptions);
        }

        #endregion
    }
}