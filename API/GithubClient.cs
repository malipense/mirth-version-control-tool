using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIClient
{
    public class GithubClient : BaseClient
    {
        private readonly string _token;
        public GithubClient(string uri, string token) :base(uri, null, null)
        {
            _token = token;
        }
        public override async Task<string> GetAsync(string path)
        {
            Uri uri = new Uri(_baseUri + path);

            try
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "mirth-tool-github-client");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);

                Console.WriteLine("Requesting data from github...");
                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nSomething went wrong!");
                Console.WriteLine(e.Message);
                return "";
            }
        }

        public async Task<string> PutAsync(string path, CommitBody body)
        {
            Uri uri = new Uri(_baseUri + path);
            try
            {               
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "mirth-tool-github-client");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);

                Console.WriteLine("Comminting changes to repository");

                JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
                jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                var jsonBody = JsonSerializer.Serialize(body, jsonOptions);

                var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(uri, httpContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nSomething went wrong!");
                Console.WriteLine(e.Message);
                return "";
            }
        }

        public override Task<string> OptionsAsync()
        {
            throw new NotImplementedException();
        }

        public override Task Authenticate()
        {
            throw new NotImplementedException();
        }
    }
}
