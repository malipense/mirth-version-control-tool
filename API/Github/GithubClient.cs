using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIClient
{
    public class GithubClient : BaseClient
    {
        private string _token;
        public GithubClient(string uri, string token) :base(uri, "application/json")
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "mirth-tool-github-client");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
        }
        public async Task<string> GetAsync(string path)
        {
            Uri uri = new Uri(_baseUri + path);

            try
            {
                Console.WriteLine("Getting list of repositories for the current user...");
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

        public async Task<string> PutAsync(string path, CommitPayload payload)
        {
            Uri uri = new Uri(_baseUri + path);
            try
            {               
                Console.WriteLine($"Comminting changes to repository: /{payload.Owner}/{payload.Repo}");

                JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
                jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                var jsonBody = JsonSerializer.Serialize(payload, jsonOptions);

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
    }
}
