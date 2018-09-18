using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Microsoft.WebCore.Helper
{
    /// <summary>
    /// http request call for web api
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// GET by <see cref="System.Net.Http.HttpClient"/> and convert to specify type use jsonconvert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns>json 反序列结果</returns>
        public static T GetJsonResponse<T>(string uri)
            where T : class, new()
        {
            var result = default(T);
            var content = GetResponse(uri, null);
            result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }

        public static T GetXmlResponse<T>(string uri)
            where T : class, new()
        {
            var result = default(T);
            var content = GetResponse(uri, null);
            result = XmlSerializeHelper.DeserializeString<T>(content);
            return result;
        }

        /// <summary>
        /// GET by <see cref="System.Net.Http.HttpClient"/>
        /// </summary>
        /// <param name="uri">request url</param>
        /// <param name="timeOutSeconds"></param>
        /// <param name="mediaType">default media type is application/json</param>
        /// <returns>response content</returns>
        public static string GetResponse(string uri, int? timeOutSeconds, string mediaType = "application/json")
        {
            if (uri.StartsWith("https", StringComparison.CurrentCultureIgnoreCase))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }

            var httpClient = new HttpClient() { MaxResponseContentBufferSize = maxResponseBufferSize };
            if (timeOutSeconds.HasValue)
            {
                httpClient.Timeout = new TimeSpan(0, 0, 0, timeOutSeconds.Value);
            }
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            var response = httpClient.GetAsync(uri).Result;
            string content = null;
            if (response.IsSuccessStatusCode)
            {
                content = response.Content.ReadAsStringAsync().Result;
            }
            return content;
        }

        /// <summary>
        /// POST by <see cref="System.Net.Http.HttpClient"/> and convert to specify type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <returns>json 反序列结果</returns>
        public static T PostJsonResponse<T>(string uri, string postData)
            where T : class, new()
        {
            var result = default(T);
            var content = PostResponse(uri, postData, null);
            result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }

        /// <summary>
        /// POST by <see cref="System.Net.Http.HttpClient"/> and convert to specify type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="xmlString"></param>
        /// <returns>xml object</returns>
        public static T PostXmlResponse<T>(string uri, string xmlString)
            where T : class, new()
        {
            T result = default(T);
            var content = PostResponse(uri, xmlString, null);
            result = XmlSerializeHelper.DeserializeString<T>(content);
            return result;
        }

        /// <summary>
        /// POST by <see cref="System.Net.Http.HttpClient"/>
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="timeOutSeconds"></param>
        /// <param name="mediaType">default media type is application/json</param>
        /// <returns>请求服务器返回的文本</returns>
        public static string PostResponse(string uri, string postData, int? timeOutSeconds, string mediaType = "application/json")
        {
            if (uri.StartsWith("https", StringComparison.CurrentCultureIgnoreCase))
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }

            var httpClient = new HttpClient() { MaxResponseContentBufferSize = maxResponseBufferSize };
            if (timeOutSeconds.HasValue)
            {
                httpClient.Timeout = new TimeSpan(0, 0, 0, timeOutSeconds.Value);
            }
            var httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            var response = httpClient.PostAsync(uri, httpContent).Result;
            string content = null;
            if (response.IsSuccessStatusCode)
            {
                content = response.Content.ReadAsStringAsync().Result;
            }
            return content;
        }

        public static bool ChedkUrlExists(string url)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "HEAD";
            req.AllowAutoRedirect = false;
            var result = false;
            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
                result = rsp.StatusCode == HttpStatusCode.OK;
                rsp.Close();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    rsp = null;
                }
            }
            return result;
        }

        /// <summary>
        /// Get by <see cref="System.Net.WebRequest"/>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string HttpGet(string url, int timeOut = 60 * 60)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.Timeout = timeOut;
            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    rsp = null;
                }
            }

            if (rsp == null) return string.Empty;
            using (var sr = new StreamReader(rsp.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                return sr.ReadToEnd();
            }
        }

        private const int maxResponseBufferSize = 1024 * 1024;
    }
}
