using System;
using System.ComponentModel;

namespace Microsoft.WebCore.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionOrderAttribute : DescriptionAttribute
    {
        public DescriptionOrderAttribute(string description)
            : base(description)
        {
        }

        public DescriptionOrderAttribute(string description, int order)
            : this(description)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}