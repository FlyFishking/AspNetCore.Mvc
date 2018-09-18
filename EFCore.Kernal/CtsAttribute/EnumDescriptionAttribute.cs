using System;
using System.Reflection;

namespace EFCore.Kernal.CtsAttribute
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumDescriptionAttribute : Attribute
    {
        public string Description { get; set; }

        public EnumDescriptionAttribute(string desc)
        {
            this.Description = desc;
        }

        public static string GetEnumDescription(Enum e)
        {
            var type = e.GetType();
            var fd = type.GetField(e.ToString());
            if (fd == null) return string.Empty;
            var attrs = fd.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            var txt = "";
            foreach (EnumDescriptionAttribute attr in attrs)
            {
                txt = attr.Description;
            }
            return txt;
        }
    }
}