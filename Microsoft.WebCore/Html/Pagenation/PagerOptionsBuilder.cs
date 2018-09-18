using System.Collections.Generic;
using System.Web.Routing;

namespace XinDaYang.RentalCars.Tools.Pagenation
{
    
    public class PagerOptionsBuilder
    {
        private readonly PagerOptions _pagerOptions;

        
        public PagerOptionsBuilder(PagerOptions pagerOptions)
        {
            _pagerOptions = pagerOptions;
        }

        
        public PagerOptionsBuilder SetActionName(string actionName)
        {
            _pagerOptions.ActionName = actionName;
            return this;
        }

        
        public PagerOptionsBuilder SetControllerName(string controllerName)
        {
            _pagerOptions.ControllerName = controllerName;
            return this;
        }

        
        public PagerOptionsBuilder AddHtmlAttribute(string key, object value)
        {
            if (_pagerOptions.HtmlAttributes == null)
            {
                _pagerOptions.HtmlAttributes=new Dictionary<string, object>();
            }
            _pagerOptions.HtmlAttributes[key] = value;
            return this;
        }

        
        public PagerOptionsBuilder SetOnPageIndexError(string handler)
        {
            _pagerOptions.OnPageIndexError = handler;
            return this;
        }

        
        public PagerOptionsBuilder AddRouteValue(string key, object value)
        {
            if (_pagerOptions.RouteValues == null)
            {
                _pagerOptions.RouteValues=new RouteValueDictionary();
            }
            _pagerOptions.RouteValues[key] = value;
            return this;
        }

        
        public PagerOptionsBuilder SetRouteName(string routeName)
        {
            _pagerOptions.RouteName = routeName;
            return this;
        }

        
        public PagerOptionsBuilder SetFirstPageRouteName(string routeName)
        {
            _pagerOptions.FirstPageRouteName = routeName;
            return this;
        }

        
        public PagerOptionsBuilder DisableAutoHide()
        {
            _pagerOptions.AutoHide = false;
            return this;
        }

        
        public PagerOptionsBuilder SetPageIndexOutOfRangeErrorMessage(string msg)
        {
            _pagerOptions.PageIndexOutOfRangeErrorMessage = msg;
            return this;
        }

        
        public PagerOptionsBuilder SetHorizontalAlign(string alignment)
        {
            _pagerOptions.HorizontalAlign = alignment;
            return this;
        }

        
        public PagerOptionsBuilder SetInvalidPageIndexErrorMessage(string msg)
        {
            _pagerOptions.InvalidPageIndexErrorMessage = msg;
            return this;
        }

        
        public PagerOptionsBuilder SetPageIndexParameterName(string prmName)
        {
            _pagerOptions.PageIndexParameterName = prmName;
            return this;
        }

        
        public PagerOptionsBuilder SetPageIndexBoxId(string id)
        {
            _pagerOptions.PageIndexBoxId = id;
            return this;
        }

        
        public PagerOptionsBuilder SetMaximumPageIndexItems(int itmes)
        {
            _pagerOptions.MaximumPageIndexItems = itmes;
            return this;
        }

        
        public PagerOptionsBuilder SetGoToButtonId(string id)
        {
            _pagerOptions.GoToButtonId = id;
            return this;
        }

        
        public PagerOptionsBuilder SetPageNumberFormatString(string format)
        {
            _pagerOptions.PageNumberFormatString = format;
            return this;
        }

        
        public PagerOptionsBuilder SetCurrentPageNumberFormatString(string format)
        {
            _pagerOptions.CurrentPageNumberFormatString = format;
            return this;
        }

        
        public PagerOptionsBuilder SetContainerTagName(string tagName)
        {
            _pagerOptions.ContainerTagName = tagName;
            return this;
        }

        
        public PagerOptionsBuilder SetPagerItemTemplate(string template)
        {
            _pagerOptions.PagerItemTemplate = template;
            return this;
        }

        
        public PagerOptionsBuilder SetNumericPagerItemTemplate(string template)
        {
            _pagerOptions.NumericPagerItemTemplate = template;
            return this;
        }

        
        public PagerOptionsBuilder SetCurrentPagerItemTemplate(string template)
        {
            _pagerOptions.CurrentPagerItemTemplate = template;
            return this;
        }

        
        public PagerOptionsBuilder SetNavigationPagerItemTemplate(string template)
        {
            _pagerOptions.NavigationPagerItemTemplate = template;
            return this;
        }

        
        public PagerOptionsBuilder SetMorePagerItemTemplate(string template)
        {
            _pagerOptions.MorePagerItemTemplate = template;
            return this;
        }

        
        public PagerOptionsBuilder SetDisabledPagerItemTemplate(string template)
        {
            _pagerOptions.DisabledPagerItemTemplate = template;
            return this;
        }

        
        public PagerOptionsBuilder AlwaysShowFirstLastPageNumber()
        {
            _pagerOptions.AlwaysShowFirstLastPageNumber = true;
            return this;
        }

        
        public PagerOptionsBuilder SetNumericPagerItemCount(int itemCount)
        {
            _pagerOptions.NumericPagerItemCount = itemCount;
            return this;
        }

        
        public PagerOptionsBuilder HidePrevNext()
        {
            _pagerOptions.ShowPrevNext = false;
            return this;
        }

        
        public PagerOptionsBuilder SetPrevPageText(string text)
        {
            _pagerOptions.PrevPageText = text;
            return this;
        }

        
        public PagerOptionsBuilder SetNextPageText(string text)
        {
            _pagerOptions.NextPageText = text;
            return this;
        }

        
        public PagerOptionsBuilder HideNumericPagerItems()
        {
            _pagerOptions.ShowNumericPagerItems = false;
            return this;
        }

        
        public PagerOptionsBuilder HideFirstLast()
        {
            _pagerOptions.ShowFirstLast = false;
            return this;
        }

        
        public PagerOptionsBuilder SetFirstPageText(string text)
        {
            _pagerOptions.FirstPageText = text;
            return this;
        }

        
        public PagerOptionsBuilder SetLastPageText(string text)
        {
            _pagerOptions.LastPageText = text;
            return this;
        }

        
        public PagerOptionsBuilder HideMorePagerItems()
        {
            _pagerOptions.ShowMorePagerItems = false;
            return this;
        }

        
        public PagerOptionsBuilder SetMorePageText(string text)
        {
            _pagerOptions.MorePageText = text;
            return this;
        }

        
        public PagerOptionsBuilder SetId(string id)
        {
            _pagerOptions.Id = id;
            return this;
        }

        
        public PagerOptionsBuilder SetCssClass(string cssClass)
        {
            _pagerOptions.CssClass = cssClass;
            return this;
        }

        
        public PagerOptionsBuilder HideDisabledPagerItems()
        {
            _pagerOptions.ShowDisabledPagerItems = false;
            return this;
        }

        
        public PagerOptionsBuilder SetMaximumPageNumber(int number)
        {
            _pagerOptions.MaximumPageNumber = number;
            return this;
        }

        
        public PagerOptionsBuilder HidePagerItems()
        {
            _pagerOptions.HidePagerItems = true;
            return this;
        }

        
        public PagerOptionsBuilder SetNavigationPagerItemsPosition(PagerItemsPosition position)
        {
            _pagerOptions.NavigationPagerItemsPosition = position;
            return this;
        }

        
        public PagerOptionsBuilder SetRouteValues(RouteValueDictionary values)
        {
            _pagerOptions.RouteValues = values;
            return this;
        }

        
        public PagerOptionsBuilder SetHtmlAttributes(IDictionary<string, object> attributes)
        {
            _pagerOptions.HtmlAttributes = attributes;
            return this;
        }
    }
}
