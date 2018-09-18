using System;
using System.Web;
using System.Web.Mvc;

namespace XinDaYang.RentalCars.Tools.Pagenation
{
    public class AjaxPager : IHtmlString
    {
        private readonly AjaxHelper _ajaxHelper;
        private readonly int _currentPageIndex;
        private readonly int _pageSize;
        private readonly int _totalItemCount;
        private PagerOptions _pagerOptions;
        private MvcAjaxOptions _ajaxOptions;

        public AjaxPager(AjaxHelper ajax, int totalItemCount, int pageSize, int pageIndex, PagerOptions pagerOptions, MvcAjaxOptions ajaxOptions)
        {
            _ajaxHelper = ajax;
            _totalItemCount = totalItemCount;
            _pageSize = pageSize;
            _currentPageIndex = pageIndex;
            _pagerOptions = pagerOptions;
            _ajaxOptions = ajaxOptions;
        }

        public AjaxPager(AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions, MvcAjaxOptions ajaxOptions)
            : this(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, pagerOptions, ajaxOptions)
        {
        }

        public AjaxPager Options(Action<PagerOptionsBuilder> builder)
        {
            if (_pagerOptions == null)
            {
                _pagerOptions = new PagerOptions();
            }
            builder(new PagerOptionsBuilder(_pagerOptions));
            return this;
        }

        public AjaxPager AjaxOptions(Action<MvcAjaxOptionsBuilder> builder)
        {
            if (_ajaxOptions == null)
            {
                _ajaxOptions = new MvcAjaxOptions();
            }
            builder(new MvcAjaxOptionsBuilder(_ajaxOptions));
            return this;
        }

        public string ToHtmlString()
        {
            var totalPageCount = (int) Math.Ceiling(_totalItemCount/(double) _pageSize);
            return new PagerBuilder(_ajaxHelper, totalPageCount, _currentPageIndex, _pagerOptions, _ajaxOptions).GenerateHtml();
        }
    }
}
