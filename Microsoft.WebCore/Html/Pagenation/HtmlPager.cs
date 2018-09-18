
using System;
using System.Web;
using System.Web.Mvc;

namespace XinDaYang.RentalCars.Tools.Pagenation
{
    
    public class HtmlPager:IHtmlString
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly int _currentPageIndex;
        private readonly int _pageSize;
        private readonly int _totalItemCount;
        private PagerOptions _pagerOptions;

        
        public HtmlPager(HtmlHelper html, int totalItemCount, int pageSize, int pageIndex,PagerOptions pagerOptions)
        {
            _htmlHelper = html;
            _totalItemCount = totalItemCount;
            _pageSize = pageSize;
            _currentPageIndex = pageIndex;
            _pagerOptions = pagerOptions;
        }

        
        public HtmlPager(HtmlHelper html, int totalItemCount, int pageSize, int pageIndex):this(html,totalItemCount,pageSize,pageIndex,null){}

        
        public HtmlPager(HtmlHelper html, IPagedList pagedList,PagerOptions pagerOptions) : this(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex,pagerOptions) { }

        
        public HtmlPager(HtmlHelper html, IPagedList pagedList):this(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex){}


        
        public HtmlPager Options(Action<PagerOptionsBuilder> builder)
        {
            if (_pagerOptions == null)
            {
                _pagerOptions = new PagerOptions();
            }
            builder(new PagerOptionsBuilder(_pagerOptions));
            return this;
        }

        
        public string ToHtmlString()
        {
            var totalPageCount = (int)Math.Ceiling(_totalItemCount / (double)_pageSize);
            return new PagerBuilder(_htmlHelper, totalPageCount, _currentPageIndex, _pagerOptions).GenerateHtml();
        }
    }

}
