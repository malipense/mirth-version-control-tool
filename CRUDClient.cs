using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace version_control_tool
{
    class CRUDClient
    {
        private readonly HttpClient _httpClient;
        private readonly CookieContainer _cookieContainer;
        private readonly string _uri;
        private readonly string _username;
        private readonly string _password;
        private bool _authenticated = false;
        public CRUDClient(string uri, string username, string password)
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
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

            return httpClient;
        }

        public async Task Authenticate()
        {
            Console.WriteLine("Authenticating user...\n");
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

                Console.WriteLine(
                    $"---------------------\n" +
                    $"|User authenticated!|\n" +
                    $"---------------------\n");

                _cookieContainer.GetCookies(uri).Cast<Cookie>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nSomething went wrong!");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        public async Task<string> Get(string endpoint)
        {
            if(!_authenticated)
                await Authenticate();
            try
            {
                Console.WriteLine("Retriving data - status:");
                HttpResponseMessage response = await _httpClient.GetAsync(_uri + endpoint);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var j = JObject.Parse(responseBody);
                var resultsAmount = j["list"].Count() > 0 ? "found" : "none";

                Console.WriteLine(
                    $"-------------------------------------------------------------\n" +
                    $"Pulling from {_uri + endpoint} \n" +
                    $"Results: {resultsAmount} \n" +
                    $"-------------------------------------------------------------\n");

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
