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
        public CRUD()
        {
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


        public async Task Post()
        {
            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username" ,"admin"),
                    new KeyValuePair<string, string>("password" ,"admin")
                });

                HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:8443/api/users/_login", content);
                response.EnsureSuccessStatusCode();
                _cookieContainer.GetCookies(new Uri("https://localhost:8443/api/users/_login")).Cast<Cookie>();

                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        public async Task Get()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:8443/api/channels");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        
    }
}
