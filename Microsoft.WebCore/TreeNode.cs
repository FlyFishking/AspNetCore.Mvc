namespace Microsoft.WebCore
{
    public class TreeNode<T>
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public T Data { get; set; }
        public bool IsLazyLoad { get; set; }
        public bool Folder { get; set; }
        public string Href { get; set; }
        public TreeNode<T> Children { get; set; }
    }
    public class SelectListItem : NameValuePair<string>
    {
        public bool Selected { get; set; }
    }

    public class SortedNameValuePair<T> : NameValuePair<T>
    {
        public int Order { get; set; }
    }
    public class NameValuePair<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public bool IsEnabled { get; set; }
    }
}