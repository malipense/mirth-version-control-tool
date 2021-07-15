using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace version_control_tool
{
    class CRUD
    {
        private readonly HttpClient _httpClient;
        private readonly CookieContainer _cookieContainer;
        private readonly string _uri;
        private readonly string _username;
        private readonly string _password;
        private bool _authenticated = false;
        public CRUD(string uri, string username, string password)
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
            return httpClient;
        }

        public async Task Authenticate()
        {
            Console.WriteLine("Authenticating...");
            Uri uri = new Uri(_uri + Endpoints.Login);
            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", _username),
                    new KeyValuePair<string, string>("password", _password)
                });

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                _authenticated = true;

                Console.WriteLine("Authenticated!");

                _cookieContainer.GetCookies(uri).Cast<Cookie>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nSomething went wrong!");
                Console.WriteLine(e.Message);
            }
        }

        public async Task<string> Get(string endpoint)
        {
            if(!_authenticated)
                await Authenticate();
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_uri + endpoint);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nSomething went wrong!");
                Console.WriteLine(e.Message);
                return "";
            }
        }

    }
}
