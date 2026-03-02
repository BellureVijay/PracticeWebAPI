using PracticeWebApi.Services.Interfaces;

namespace PracticeWebApi.Utilities
{
    public class ApiPollingWorker:BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ApiPollingWorker> _logger;
        public ApiPollingWorker(IServiceScopeFactory call,ILogger<ApiPollingWorker> logger)
        {
             _serviceScopeFactory= call;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(20));
            try
            {

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                    var apiService = scope.ServiceProvider.GetRequiredService<IExternalAPICall>();
                var result = await apiService.GetCatPhotosAsync();
                _logger.LogInformation("Api Response:{result} ",result);

            }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error calling external API:{ex}",ex);
            }

        }
    }
}
