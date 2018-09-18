using System;
using System.ComponentModel;
using System.Reflection;

namespace EFCore.Kernal.CtsAttribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNameResourceAttribute : DisplayNameAttribute
    {
        public override string DisplayName { get; }

        public DisplayNameResourceAttribute(string key)
            : base(GetMessageFromResource(key))
        {
            DisplayName = key;
        }

        private static string GetMessageFromResource(string resourceId)
        {
            return null;
        }
    }
}
