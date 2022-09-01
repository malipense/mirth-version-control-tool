using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIClient
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
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            clientHandler.CookieContainer = _cookieContainer;

            HttpClient httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(_mediaType)
                );

            return httpClient;
        }
    }
}
