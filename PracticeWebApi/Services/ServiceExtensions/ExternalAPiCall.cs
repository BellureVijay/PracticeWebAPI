using PracticeWebApi.Services.Interfaces;
using System.Net.Http;

namespace PracticeWebApi.Services.ServiceExtensions
{
    public class ExternalAPiCall : IExternalAPICall
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public ExternalAPiCall(IHttpClientFactory client)
        {
            _httpClientFactory = client;
        }
        public async Task<string> GetCatPhotosAsync()
        {
            var client = _httpClientFactory.CreateClient("ExternalApi");
            var response = await client.GetAsync("v1/forecast?latitude=52.52&longitude=13.41&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
