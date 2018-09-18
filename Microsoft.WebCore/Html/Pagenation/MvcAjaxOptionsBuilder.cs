
namespace XinDaYang.RentalCars.Tools.Pagenation
{
    public class MvcAjaxOptionsBuilder
    {
        private readonly MvcAjaxOptions _ajaxOptions;

        public MvcAjaxOptionsBuilder(MvcAjaxOptions ajaxOptions)
        {
            _ajaxOptions = ajaxOptions;
        }

        public MvcAjaxOptionsBuilder SetUpdateTargetId(string targetId)
        {
            _ajaxOptions.UpdateTargetId = targetId;
            return this;
        }

        public MvcAjaxOptionsBuilder SetHttpMethod(string method)
        {
            _ajaxOptions.HttpMethod = method;
            return this;
        }

        public MvcAjaxOptionsBuilder SetOnBegin(string name)
        {
            _ajaxOptions.OnBegin = name;
            return this;
        }

        public MvcAjaxOptionsBuilder SetOnSuccess(string name)
        {
            _ajaxOptions.OnSuccess = name;
            return this;
        }

        public MvcAjaxOptionsBuilder SetOnComplete(string name)
        {
            _ajaxOptions.OnComplete = name;
            return this;
        }

        public MvcAjaxOptionsBuilder SetOnFailure(string name)
        {
            _ajaxOptions.OnFailure = name;
            return this;
        }

        public MvcAjaxOptionsBuilder SetLoadingElementId(string id)
        {
            _ajaxOptions.LoadingElementId = id;
            return this;
        }

        public MvcAjaxOptionsBuilder SetLoadingElementDuration(int duration)
        {
            _ajaxOptions.LoadingElementDuration = duration;
            return this;
        }

        public MvcAjaxOptionsBuilder SetConfirm(string confirm)
        {
            _ajaxOptions.Confirm = confirm;
            return this;
        }

        public MvcAjaxOptionsBuilder EnablePartialLoading()
        {
            _ajaxOptions.EnablePartialLoading = true;
            return this;
        }

        public MvcAjaxOptionsBuilder SetDataFormId(string id)
        {
            _ajaxOptions.DataFormId = id;
            return this;
        }

        public MvcAjaxOptionsBuilder DisallowCache()
        {
            _ajaxOptions.AllowCache = false;
            return this;
        }

        public MvcAjaxOptionsBuilder DisableHistorySupport()
        {
            _ajaxOptions.EnableHistorySupport = false;
            return this;
        }
    }
}
