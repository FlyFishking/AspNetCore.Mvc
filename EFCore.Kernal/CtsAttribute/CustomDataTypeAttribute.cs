using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EFCore.Kernal.CtsAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomDataTypeAttribute : ValidationAttribute
    {
        private readonly RegularType regType;

        public CustomDataTypeAttribute(CustomDataType dataType)
        {
            this.InputType = dataType;
            switch (dataType)
            {
                case CustomDataType.DateTime:
                    regType = RegularType.IsDateTime;
                    break;
                case CustomDataType.EmailAddress:
                    regType = RegularType.IsEmail;
                    break;
                case CustomDataType.Int:
                    regType = RegularType.IsInt;
                    break;
                case CustomDataType.Chinese:
                    regType = RegularType.IsAllChinese;
                    break;
            }
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.ToString() == string.Empty)
                {
                    return true;
                }
                return RegularExpressions.IsMatch(regType, value.ToString());
            }
            return true;
        }

        public CustomDataType InputType { get; }

        //        public System.Collections.Generic.IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //        {
        //            ModelClientValidationRule validationRule = new ModelClientValidationRule() { ValidationType = "customdatatype", ErrorMessage = FormatErrorMessage(metadata.DisplayName) };
        //            validationRule.ValidationParameters.Add("strregex", RegularExpressions.GetExpression(regType).ToString());
        //            yield return validationRule;
        //        }

        /// <summary>
        /// 根据正则获取指定位置的值
        /// <example>
        /// <![CDATA[
        /// string key = @"U_CacheLearnnet_Entity|1121131";
        /// var aa = GetMatchsByKey(key, new string[] { @"^\w{1}_CacheLearnnet_Entity\|(?<get>\d*?)$" }, "get");
        /// ]]>
        /// </example>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> GetMatchsByKey(string str, string[] pattern, string key)
        {
            var list = new List<string>();
            foreach (var p in pattern)
            {
                var matchs = new Regex(p, RegexOptions.Singleline | RegexOptions.IgnoreCase).Matches(str);
                foreach (Match m in matchs)
                {
                    var g = m.Groups[key];
                    if (g != null)
                    {
                        list.Add(g.Value);
                    }
                }
            }
            return list;
        }
    }
}