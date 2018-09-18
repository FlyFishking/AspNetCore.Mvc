using System;
using System.Globalization;

namespace Microsoft.WebCore.Extention
{
    public static partial class StringExtension
    {
        /// <summary>
        /// �Դ��ݵĲ����ַ������д�����ֹע��ʽ����
        /// </summary>
        /// <param name="sqlQuery">���ݵĲ����ַ���</param>
        /// <returns>String</returns>
        public static string ClearSqlQueryString(string sqlQuery)
        {
            sqlQuery = sqlQuery.Trim();
            sqlQuery = sqlQuery.Replace("'", "''");
            sqlQuery = sqlQuery.Replace(";--", "");
            sqlQuery = sqlQuery.Replace("=", "");
            sqlQuery = sqlQuery.Replace(" or ", "", StringComparison.CurrentCultureIgnoreCase);
            sqlQuery = sqlQuery.Replace(" and ", "", StringComparison.CurrentCultureIgnoreCase);
            return sqlQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">length of bytes</param>
        /// <returns></returns>
        public static string FormatSize(long size)
        {
            var result = "";
            if (size >= 1024 * 1024 * 1024)
                result = Convert.ToDecimal(size / (1024 * 1024 * 1024)).ToString(CultureInfo.InvariantCulture) + "GB";
            else if (size >= 1024 * 1024)
                result = Convert.ToDecimal(size / (1024 * 1024)).ToString(CultureInfo.InvariantCulture) + "MB";
            else if (size >= 1024)
                result = Convert.ToDecimal(size / (1024 * 1024)).ToString(CultureInfo.InvariantCulture) + "KB";
            else
                result = size + "Bytes";
            return result;
        }
    }
}