using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EFCore.Kernal.CtsAttribute
{
    public static class RegularExpressions
    {
        public static Regex GetExpression(RegularType expressionType)
        {
            return expressions[expressionType];
        }

        public static string GetExpressionPattern(RegularType expressionType)
        {
            return expressions[expressionType].ToString();
        }

        public static bool IsMatch(RegularType expressionType, string input)
        {
            var regex = expressions[expressionType];
            return regex.IsMatch(input);
        }

        private static Dictionary<RegularType, Regex> expressions
        {
            get
            {
                var dictionary = new Dictionary<RegularType, Regex>();
                dictionary.Add(RegularType.IsEmail, new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.EmailReplace, new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.IsUrl, new Regex("^https?://(?:[^./\\s'\"<)\\]]+\\.)+[^./\\s'\"<\")\\]]+(?:/[^'\"<]*)*$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.UrlReplace, new Regex(@"(http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=\+]*)?)", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.HyperlinkReplace, new Regex("<a[^>]+>([^<]+)</a>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.IsDateTime, new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.MultiNbspReplace, new Regex("(&nbsp;)+", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.MultiCommaReplace, new Regex("((&nbsp;)*(,|，)(&nbsp;)*)+", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.IsInt, new Regex(@"^-?[1-9]\d*$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase));
                dictionary.Add(RegularType.UserName, new Regex(@"^[a-zA-Z]{1}[a-zA-Z0-9\.]+$", RegexOptions.Compiled));
                dictionary.Add(RegularType.IsNumber, new Regex(@"^[+-]?[0123456789]*[.]?[0123456789]*$", RegexOptions.Compiled));
                dictionary.Add(RegularType.IsDecimal, new Regex(@"^[0-9]+/.?[0-9]{0,2}$", RegexOptions.Compiled));
                dictionary.Add(RegularType.IsIntWithZero, new Regex(@"^\\d+$", RegexOptions.Compiled));
                dictionary.Add(RegularType.IsIP, new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$", RegexOptions.Compiled));
                dictionary.Add(RegularType.IsTelephone, new Regex(@"^((\d{3,4})|\d{3,4}-|\s)?\d{8}$", RegexOptions.Compiled));
                dictionary.Add(RegularType.IsMobilePhone, new Regex(RegexIsMobile, RegexOptions.Compiled));
                dictionary.Add(RegularType.IsPostcode, new Regex(@"^[1-9]{1}(\d){5}$", RegexOptions.Compiled));
                dictionary.Add(RegularType.IsAllChinese, new Regex(RegexIsAllChiness, RegexOptions.Compiled));
                return dictionary;
            }
        }
        public const string RegexValidPassword = @"/^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,32}$/";
        public const string RegexIsMobile = @"(86)*0*1\d{10}$";
        public const string RegexIsAllChiness = @"^([\u4e00-\u9fa5]*)$";
        //中文、英文、数字
        public const string RegexIsNoSpecialChars = @"^([\u4e00-\u9fa5a-zA-Z0-9]+)$";
        public const string RegexIsPostcode = @"^([\u4e00-\u9fa5]*)$";
    }
}
