using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace version_control_tool
{
    public class Request
    {
        CookieContainer cookieContainer = new CookieContainer();
        public HttpWebResponse CreateRequest(string requestType, string url, string encodedBody)
        {

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = requestType;
            request.AllowAutoRedirect = false;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.CookieContainer = cookieContainer;

            ServicePointManager.ServerCertificateValidationCallback = delegate (
            Object obj, X509Certificate certificate, X509Chain chain,
             SslPolicyErrors errors)
            {
                return (true);
            };

            if (requestType == "POST")
            {
                byte[] data = Encoding.ASCII.GetBytes(encodedBody);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }

            if (requestType == "PUT")
            {
                byte[] data = Encoding.ASCII.GetBytes(encodedBody);
                request.ContentType = "application/xml";
                request.ContentLength = data.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();

            if(cookieContainer.Count == 0)
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    foreach (Cookie cookie in response.Cookies)
                    {
                        cookieContainer.Add(cookie);
                    }
                }
            }
            
            return response;
        }
    }
}
