using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NextGen.Cli
{
    public class MirthHttpClient : BaseClient
    {
        private string _username;
        private string _password;
        public MirthHttpClient(string baseUri, string username, string password) :base(baseUri, "application/xml")
        {
            _username = username;
            _password = password;
        }

        
        public async Task<HttpResponseMessage> Authenticate()
        {
            Uri uri = new Uri(_baseUri + Endpoints.Login);

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", _username),
                    new KeyValuePair<string, string>("password", _password)
                });

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
            
            if(response.StatusCode == HttpStatusCode.OK)
                _cookieContainer.GetCookies(uri).Cast<Cookie>();

            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var authResponse = await Authenticate();

            if (authResponse.StatusCode != HttpStatusCode.OK)
                return authResponse;

            else 
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUri + endpoint);

                return response;
            }
        }
    }
}
