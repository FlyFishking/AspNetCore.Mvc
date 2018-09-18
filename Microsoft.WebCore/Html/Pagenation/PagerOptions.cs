using System.Collections.Generic;
using System.Web.Routing;
using XinDaYang.RentalCars.Resources;
using XinDaYang.RentalCars.Tools.Pagenation;

namespace XinDaYang.RentalCars.Tools.Pagenation
{
    
    public class PagerOptions
    {
        
        public PagerOptions(bool useBootstrap = false)
        {
            AutoHide = true;
            PageIndexParameterName = "pageIndex";
            NumericPagerItemCount = 10;
            AlwaysShowFirstLastPageNumber = false;
            ShowPrevNext = true;
            PrevPageText = PagenationResources.PrevPageText;
            NextPageText = PagenationResources.NextPageText;
            ShowNumericPagerItems = true;
            ShowFirstLast = true;
            FirstPageText = PagenationResources.FirstPageText;
            LastPageText = PagenationResources.LastPageText;
            ShowMorePagerItems = true;
            MorePageText = "...";
            ShowDisabledPagerItems = true;
            //PagerItemsSeparator = "&nbsp;&nbsp;";
            MaximumPageIndexItems = 20;
            ContainerTagName = "div";
            InvalidPageIndexErrorMessage = PagenationResources.InvalidPageIndexErrorMessage;
            PageIndexOutOfRangeErrorMessage = PagenationResources.PageIndexOutOfRangeErrorMessage;
            MaximumPageNumber = 0;
            FirstPageRouteName = null;
            if (useBootstrap)
            {
                _containerTagName = "ul";
                CssClass = "pagination";
                CurrentPagerItemTemplate = "<li class=\"active\"><a data-pageindex=\"{0}\" href=\"#\">{0}</a></li>";
                DisabledPagerItemTemplate = "<li class=\"disabled\"><a>{0}</a></li>";
                PagerItemTemplate = "<li>{0}</li>";
            }
        }

        
        public string ActionName { get; set; }

        
        public string ControllerName { get; set; }

        
        public string RouteName { get; set; }

        
        public RouteValueDictionary RouteValues { get; set; }

        
        public IDictionary<string,object> HtmlAttributes { get; set; }

        
        public string FirstPageRouteName { get; set; }

        
        public bool AutoHide { get; set; }


        
        public string PageIndexOutOfRangeErrorMessage { get; set; }

        
        public string InvalidPageIndexErrorMessage { get; set; }

        
        public string PageIndexParameterName { get; set; }

        
        public string PageIndexBoxId { get; set; }

        
        public string GoToButtonId { get; set; }
        
        
        public int MaximumPageIndexItems { get; set; }
        

        
        public string PageNumberFormatString { get; set; }
       

        
        public string CurrentPageNumberFormatString { get; set; }

        private string _containerTagName;
        

        
        public string ContainerTagName
        {
            get
            {
                return _containerTagName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new System.ArgumentException(PagenationResources.ContainerTagNameCannotBeNull);
                _containerTagName = value;
            }
        }


        
        public string PagerItemTemplate { get; set; }


        
        public string NumericPagerItemTemplate { get; set; }


        
        public string CurrentPagerItemTemplate { get; set; }


        
        public string NavigationPagerItemTemplate { get; set; }


        
        public string MorePagerItemTemplate { get; set; }


        
        public string DisabledPagerItemTemplate { get; set; }


        
        public bool AlwaysShowFirstLastPageNumber { get; set; }

        
        public int NumericPagerItemCount { get; set; }


        
        public bool ShowPrevNext { get; set; }
        

        
        public string PrevPageText { get; set; }
       

        
        public string NextPageText { get; set; }
        

        
        public bool ShowNumericPagerItems { get; set; }

        
        public bool ShowFirstLast { get; set; }
        

        
        public string FirstPageText { get; set; }
        

        
        public string LastPageText { get; set; }
        

        
        public bool ShowMorePagerItems { get; set; }
        

        
        public string MorePageText { get; set; }
        

        
        public string Id { get; set; }
        

        
        public string HorizontalAlign { get; set; }
      

        
        public string CssClass { get; set; }

        
        public bool ShowDisabledPagerItems { get; set; }
        

        
        public int MaximumPageNumber { get; set; }

        
        public bool HidePagerItems { get; set; }

        private PagerItemsPosition _navPagerItemsPosition = PagerItemsPosition.BothSide;

        
        public PagerItemsPosition NavigationPagerItemsPosition { get { return _navPagerItemsPosition; } set{_navPagerItemsPosition = value;} }

        
        public string OnPageIndexError { get; set; }
    }
}