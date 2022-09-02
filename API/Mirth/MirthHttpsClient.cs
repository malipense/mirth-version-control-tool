using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace APIClient
{
    public class MirthHttpsClient : BaseClient
    {
        private string _username;
        private string _password;
        private bool _authenticated = false;
        public MirthHttpsClient(string baseUri, string username, string password) :base(baseUri, "application/xml")
        {
            _username = username;
            _password = password;
        }

        
        public async Task Authenticate()
        {
            Console.WriteLine("Authenticating user...\n");
            Uri uri = new Uri(_baseUri + Endpoints.Login);
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

        public async Task<string> GetAsync(string endpoint)
        {
            if(!_authenticated)
                await Authenticate();
            try
            {
                Console.WriteLine("Retriving data - status:");
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUri + endpoint);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var j = XDocument.Parse(responseBody);
                var resultsAmount = j.Root.Elements().ToArray().Count();

                Console.WriteLine(
                    $"-------------------------------------------------------------\n" +
                    $"Pulling from {_baseUri + endpoint} \n" +
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
