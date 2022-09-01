using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace APIClient
{
    public class XMLWebRequest
    {
        private string URL { get; set; }
        private CookieContainer cookieContainer = new CookieContainer();
        public XMLWebRequest(string URL)
        {
            this.URL = URL;
        }


        public HttpWebRequest CreateRequest(string requestType, string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = requestType;
            request.AllowAutoRedirect = false;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.CookieContainer = cookieContainer;

            ServicePointManager.ServerCertificateValidationCallback = delegate (
            object obj, X509Certificate certificate, X509Chain chain,
             SslPolicyErrors errors)
            {
                return (true);
            };

            return request;
        }

        
        public void Authenticate(string username, string password)
        {
            string encodedLogin = $"username={username}&password={password}";
            UpdateCookies(Post(encodedLogin));
        }

        public HttpWebResponse Post(string encodedContent)
        {
            var request = CreateRequest("POST", URL + Endpoints.Login);
            byte[] data = Encoding.ASCII.GetBytes(encodedContent);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            return (HttpWebResponse)request.GetResponse();
        }

        public HttpWebResponse Put()
        {
            /*
            if (requestType == "PUT")
            {
                byte[] data = Encoding.ASCII.GetBytes(encodedBody);
                request.ContentType = "application/xml";
                request.ContentLength = data.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }
            */
            throw new NotImplementedException();
        }

        private void UpdateCookies(HttpWebResponse response)
        {
            if (cookieContainer.Count == 0)
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    foreach (Cookie cookie in response.Cookies)
                    {
                        cookieContainer.Add(cookie);
                    }
                }
            }
        }

    }
}
