using PracticeProjectWebAPI.Models.DTO;
using PracticeProjectWebAPI.services.Interfaces;

namespace PracticeProjectWebAPI.services.ServiceExtension
{
    public class ProductService : IProductService
    {
        private readonly IExternalApiClient _apiClient;

        public ProductService(IExternalApiClient apiClient)
        {
            _apiClient = apiClient;   
        }
        public async Task<ProductResponse?> GetProduct(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            string url = $"products?limit={pageSize}&skip={skip}";
            return await _apiClient.GetAsync<ProductResponse>("DummyProductApi", url);
        }
    }
}
