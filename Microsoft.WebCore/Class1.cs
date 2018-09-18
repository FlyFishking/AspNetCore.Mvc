using System.Text.RegularExpressions;
using System.Web;

namespace Microsoft.WebCore
{
    public class HttpCutsomerRequest
    {
        /// <summary>
        /// 获取浏览器版本号
        /// </summary>
        /// <returns></returns>
        public static string GetBrowser()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Browser + bc.Version;
        }


        //        /// <summary>
        //        /// 获取操作系统版本号
        //        /// </summary>
        //        /// <returns></returns>
        //        public static string GetOSVersion()
        //        {
        //            //UserAgent 
        //            var userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
        //
        //            var osVersion = "未知";
        //
        //            if (userAgent.Contains("NT 6.1"))
        //            {
        //                osVersion = "Windows 7";
        //            }
        //            else if (userAgent.Contains("NT 6.0"))
        //            {
        //                osVersion = "Windows Vista/Server 2008";
        //            }
        //            else if (userAgent.Contains("NT 5.2"))
        //            {
        //                osVersion = "Windows Server 2003";
        //            }
        //            else if (userAgent.Contains("NT 5.1"))
        //            {
        //                osVersion = "Windows XP";
        //            }
        //            else if (userAgent.Contains("NT 5"))
        //            {
        //                osVersion = "Windows 2000";
        //            }
        //            else if (userAgent.Contains("NT 4"))
        //            {
        //                osVersion = "Windows NT4";
        //            }
        //            else if (userAgent.Contains("Me"))
        //            {
        //                osVersion = "Windows Me";
        //            }
        //            else if (userAgent.Contains("98"))
        //            {
        //                osVersion = "Windows 98";
        //            }
        //            else if (userAgent.Contains("95"))
        //            {
        //                osVersion = "Windows 95";
        //            }
        //            else if (userAgent.Contains("Mac"))
        //            {
        //                osVersion = "Mac";
        //            }
        //            else if (userAgent.Contains("Unix"))
        //            {
        //                osVersion = "UNIX";
        //            }
        //            else if (userAgent.Contains("Linux"))
        //            {
        //                osVersion = "Linux";
        //            }
        //            else if (userAgent.Contains("SunOS"))
        //            {
        //                osVersion = "SunOS";
        //            }
        //            return osVersion;
        //        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables[""];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "0.0.0.0";
            }
            return "192.168.1.130";
        }


        ///  <summary>  
        ///  取得客户端真实IP。如果有代理则取第一个非内网地址  
        ///  </summary>  
        public static string GetIPAddress
        {
            get
            {
                var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理  
                    if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式  
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。  
                            result = result.Replace("  ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                        && temparyip[i].Substring(0, 3) != "10."
                                        && temparyip[i].Substring(0, 7) != "192.168"
                                        && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];        //找到不是内网的地址  
                                }
                            }
                        }
                        else if (IsIPAddress(result))  //代理即是IP格式  
                            return result;
                        else
                            result = null;        //代理中的内容  非IP，取IP  
                    }

                }

                string ipAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != string.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }
        }


        ///  <summary>
        ///  判断是否是IP地址格式  0.0.0.0
        ///  </summary>
        ///  <param  name="str1">待判断的IP地址</param>
        ///  <returns>true  or  false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15) return false;

            const string regFormat = @"^d{1,3}[.]d{1,3}[.]d{1,3}[.]d{1,3}$";

            var regex = new Regex(regFormat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }


        /// <summary>
        /// 获取公网IP
        /// </summary>
        /// <returns></returns>
        public static string GetNetIP()
        {
            string tempIP = "";
            try
            {
                System.Net.WebRequest wr = System.Net.WebRequest.Create("http://city.ip138.com/ip2city.asp");
                System.IO.Stream s = wr.GetResponse().GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(s, System.Text.Encoding.GetEncoding("gb2312"));
                string all = sr.ReadToEnd(); //读取网站的数据

                int start = all.IndexOf("[") + 1;
                int end = all.IndexOf("]", start);
                tempIP = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch
            {
                if (System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.Length > 1)
                    tempIP = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
                if (string.IsNullOrEmpty(tempIP))
                    return GetIP();
            }
            return tempIP;
        }

        private static string getIp()
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        //        Request.ServerVariables["HTTP_VIA"] ,ServerVariables["HTTP_X_FORWARDED_FOR"],Request.ServerVariables["REMOTE_ADDR"] 的值分下面几种情况:
        //一、没有使用代理服务器的情况：
        //
        //      REMOTE_ADDR = 用户的 IP
        //      HTTP_VIA = 没数值或不显示
        //      HTTP_X_FORWARDED_FOR = 没数值或不显示
        //二、使用透明代理服务器的情况：Transparent Proxies
        //
        //      REMOTE_ADDR = 最后一个代理服务器 IP
        //      HTTP_VIA = 代理服务器 IP
        //      HTTP_X_FORWARDED_FOR = 用户的真实 IP ，经过多个代理服务器时，这个值类似如下：203.98.182.163, 203.98.182.163, 203.129.72.215。
        //
        //   这类代理服务器还是将您的信息转发给您的访问对象，无法达到隐藏真实身份的目的。
        //
        //三、使用普通匿名代理服务器的情况：Anonymous Proxies
        //
        //      REMOTE_ADDR = 最后一个代理服务器 IP
        //      HTTP_VIA = 代理服务器 IP
        //      HTTP_X_FORWARDED_FOR = 代理服务器 IP ，经过多个代理服务器时，这个值类似如下：203.98.182.163, 203.98.182.163, 203.129.72.215。
        //
        //   隐藏了您的真实IP，但是向访问对象透露了您是使用代理服务器访问他们的。 
        //四、使用欺骗性代理服务器的情况：Distorting Proxies
        //
        //      REMOTE_ADDR = 代理服务器 IP
        //      HTTP_VIA = 代理服务器 IP
        //      HTTP_X_FORWARDED_FOR = 随机的 IP ，经过多个代理服务器时，这个值类似如下：203.98.182.163, 203.98.182.163, 203.129.72.215。
        //
        //   告诉了访问对象您使用了代理服务器，但编造了一个虚假的随机IP代替您的真实IP欺骗它。
        //
        //所以getIp()也不是最好的,因为可以编造一个假的IP,具体做法去看http://www.cnblogs.com/kingthy/archive/2007/11/24/970783.html
        //
        //总结:
        //"Request.UserHostAddress"是可信的.但是这样的话却又获取不了那些已使用了代理服务器的用户真实IP地址(因为在这时Request.UserHostAddress获取到的就是这代理服务器的IP).. 
        //getIp()又有安全隐患
        //具体怎么做就要看自己选择了.
    }
}
