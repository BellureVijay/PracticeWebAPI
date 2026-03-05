using PracticeProjectWebAPI.Models.DTO;

namespace PracticeProjectWebAPI.services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse?> GetProduct(int page,int pageSize);

    }
}
