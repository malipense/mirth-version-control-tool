using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NextGen.Cli
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly CookieContainer _cookieContainer;
        protected readonly string _baseUri;
        protected readonly string _mediaType;

        public BaseClient(string baseUri, string mediaType)
        {
            _baseUri = baseUri;
            _mediaType = mediaType;
            _cookieContainer = new CookieContainer();
            _httpClient = GenerateClient();
        }
        private HttpClient GenerateClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = ValidateCertificate;
            clientHandler.CookieContainer = _cookieContainer;

            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(_mediaType)
                );

            return httpClient;
        }

        private bool ValidateCertificate(HttpRequestMessage req, X509Certificate2 cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
