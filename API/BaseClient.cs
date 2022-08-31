using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIClient
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly CookieContainer _cookieContainer;
        protected readonly string _uri;
        protected readonly string _username;
        protected readonly string _password;
        protected bool _authenticated = false;

        public BaseClient(string uri, string username, string password)
        {
            _uri = uri;
            _username = username;
            _password = password;
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
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml")
                );

            return httpClient;
        }

        public abstract Task Authenticate();
        public abstract Task<string> GetAsync(string endpoint);
        public abstract Task<string> OptionsAsync();
    }
}
