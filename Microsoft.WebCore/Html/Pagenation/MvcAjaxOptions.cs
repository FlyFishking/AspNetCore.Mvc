namespace XinDaYang.RentalCars.Tools.Pagenation
{
    public class MvcAjaxOptions : System.Web.Mvc.Ajax.AjaxOptions
    {
        public bool EnablePartialLoading { get; set; }

        public string DataFormId { get; set; }

        private bool _allowCache = true;

        public new bool AllowCache { get { return _allowCache; } set { _allowCache = value; } }

        private bool _enableHistorySupport = true;

        public bool EnableHistorySupport { get { return _enableHistorySupport; } set { _enableHistorySupport = value; } }

    }
}
