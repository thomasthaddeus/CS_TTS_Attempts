using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IAM
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(string baseAddress)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}