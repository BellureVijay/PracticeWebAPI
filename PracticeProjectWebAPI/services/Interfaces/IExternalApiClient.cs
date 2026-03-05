namespace PracticeProjectWebAPI.services.Interfaces
{
    public interface IExternalApiClient
    {
        Task<T?> GetAsync<T>(string clientName,string url);
        Task<TResponse> PostAsycn<TRequest, TResponse>(string clientName,string url ,TRequest data);
    }
}
