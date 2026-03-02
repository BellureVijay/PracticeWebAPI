using PracticeWebApi.Services.Interfaces;

namespace PracticeWebApi.Services.ServiceExtensions
{
    public class ServiceInteraction : IServiceInteraction
    {
        private readonly HttpClient _client;
        public ServiceInteraction(HttpClient client)
        {
            _client = client;
        }
        public async Task<string> CallOtherService()
        {
            var response = await _client.GetAsync("http://localhost:5148/WeatherForecast");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
