using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace APIClient
{
    public class MirthHttpClient : BaseClient
    {
        public MirthHttpClient(string uri, string username, string password) :base(uri, username, password)
        {   }

        
        public override async Task Authenticate()
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

        public override async Task<string> GetAsync(string endpoint)
        {
            if(!_authenticated)
                await Authenticate();
            try
            {
                Console.WriteLine("Retriving data - status:");
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUri + endpoint);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                //var j = JObject.Parse(responseBody);
                //var resultsAmount = j["list"].Count() > 0 ? "found" : "none";

                var j = XDocument.Parse(responseBody);
                var resultsAmount = j.Root.Elements().ToArray().Count() > 0 ? "found" : "none";

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

        public override async Task<string> OptionsAsync()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Options;
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nSomething went wrong!");
                Console.WriteLine(e.Message);
                return "";
            }
        }
    }
}
