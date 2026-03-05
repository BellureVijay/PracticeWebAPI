using PracticeProjectWebAPI.services.Interfaces;

namespace PracticeProjectWebAPI.services.ServiceExtension
{
    public class ExternalApiClient : IExternalApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ExternalApiClient> _logger;
        public ExternalApiClient(IHttpClientFactory httpClientFactory,ILogger<ExternalApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<T?> GetAsync<T>(string clientName, string url)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("External API Call failed {statusCode}", response.StatusCode);
                return default;
            }
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse> PostAsycn<TRequest, TResponse>(string clientName, string url, TRequest data)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var response = await client.PostAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("External API Post Failed {statusCode}", response.StatusCode);
                return default;
            }
            return await response.Content.ReadFromJsonAsync<TResponse>();

        }
    }
}
