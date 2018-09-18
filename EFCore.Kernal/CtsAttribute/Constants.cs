namespace EFCore.Kernal.CtsAttribute
{

    public enum RegularType
    {
        IsUrl,
        IsDateTime,
        IsEmail,
        EmailReplace,
        UrlReplace,
        HyperlinkReplace,
        UserName,
        MultiNbspReplace, //空格
        MultiCommaReplace, //逗号
        IsInt,//整数 
        IsNumber,//数字
        IsDecimal,//小数
        IsIntWithZero,//非负数
        IsIP,
        IsTelephone,
        IsMobilePhone,
        IsPostcode,
        IsAllChinese
    }

    public enum CustomDataType
    {
        DateTime,
        EmailAddress,
        Int,
        Chinese,
        /// <summary>
        /// 分段计价
        /// </summary>
        StepPrice
    }

    public static class StreamContentType
    {
        public const string Html = "text/html";
        public const string Text = "text/plain";
        public const string ImgGif = "image/gif";
        public const string ImgJpeg = "image/jpeg";
        public const string ImgTiff = "image/tiff";
        public const string DocWord = "application/msword";
        public const string DocRtf = "application/rtf";
        public const string DocExcel = "application/vnd.ms-excel";
        public const string DocPpt = "application/ms-powerpoint";
        public const string DocPdf = "application/pdf";
        public const string DocZip = "application/zip";
    }
    public class Constants
    {
     
    }

}