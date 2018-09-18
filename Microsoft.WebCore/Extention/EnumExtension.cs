using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Microsoft.WebCore.Extention
{
    public static class EnumExtension
    {
        /// <summary>
        /// get description
        /// </summary>
        /// <param name="enumeration"> </param>
        /// <returns>the desc of enumeration,if not founde return empty</returns>
        public static string GetEnumDescription(this Enum enumeration)
        {
            return GetEnumDescription(enumeration.GetType(), enumeration.ToString());
        }

        /// <summary>
        /// get desc information  by name
        /// </summary>
        /// <param name="type">type of enum</param>
        /// <param name="attrName">name of enum item</param>
        /// <returns></returns>
        private static string GetEnumDescription(Type type, string attrName)
        {
            if (string.IsNullOrEmpty(attrName))
                throw new ArgumentNullException(nameof(attrName));
            var filedInfo = type.GetField(attrName);
            if (filedInfo == null)
                return null;
            var attr = filedInfo.GetCustomAttributes<DescriptionAttribute>().ToArray();
            return attr.Any() ? attr[0].Description : attrName;
        }

        /// <summary>
        /// get desc information  by name
        /// </summary>
        /// <param name="attrName">name of enum item</param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(string attrName)
        {
            var attrs = GetEnumAttributes<T, DescriptionAttribute>(attrName).ToArray();
            return attrs.Any() ? attrs[0].Description : attrName;
        }

        /// <summary>
        /// Get the specify attribute of TEnum fields
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="attrName">The name of enum filed,if null which find all attributes of all the fileds</param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> GetEnumAttributes<TEnum, TAttribute>(string attrName = null)
            where TAttribute : Attribute
        {
            var type = typeof(TEnum);
            if (string.IsNullOrEmpty(attrName))
            {
                var attrs = type.GetFields().Select(t => t.GetCustomAttributes<TAttribute>());
                foreach (var attributes in attrs) foreach (var attribute in attributes)
                        yield return attribute;
            }
            else
            {
                var filedInfo = type.GetField(attrName);
                if (filedInfo == null)
                    yield break;
                var attrs = filedInfo.GetCustomAttributes<TAttribute>().ToArray();
                foreach (var attribute in attrs)
                    yield return attribute;
            }
        }

        /// <summary>
        /// Get enum name by description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumDesc"></param>
        /// <returns></returns>
        public static T GetEnumNameByDescription<T>(string enumDesc)
        {
            var type = typeof(T);
            foreach (var field in type.GetFields())
            {
                var desc = field.GetCustomAttributes<DescriptionAttribute>().ToArray();
                if (desc.IsEmpty())
                    continue;
                if (desc[0].Description == enumDesc)
                    return (T)field.GetValue(null);
                else if (field.Name == enumDesc)
                    return (T)field.GetValue(null);
            }
            return default(T);
            //            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", enumDesc), "enumDesc");
        }
    }
}