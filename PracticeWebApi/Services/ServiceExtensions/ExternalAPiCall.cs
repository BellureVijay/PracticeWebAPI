using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using PracticeWebApi.Services.Interfaces;
using System.Net.Http;

namespace PracticeWebApi.Services.ServiceExtensions
{
    public class ExternalAPiCall : IExternalAPICall
    {
        public readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public ExternalAPiCall(IHttpClientFactory client,IMemoryCache cache)
        {
            _httpClientFactory = client;
            _cache = cache;
        }
        public async Task<string> GetCatPhotosAsync()
        {
            var cacheKey = "ExternalApi";
            var ExternalAPiResponse = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
            var client = _httpClientFactory.CreateClient("ExternalApi");
            var response = await client.GetAsync("v1/forecast?latitude=52.52&longitude=13.41&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m");
            response.EnsureSuccessStatusCode();
                //Maximum 20 seconds lifetime
                //But if not accessed for 10 seconds, it expires early
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SlidingExpiration = TimeSpan.FromSeconds(10);
            return await response.Content.ReadAsStringAsync();

            });
            return ExternalAPiResponse;
        }
    }
}
