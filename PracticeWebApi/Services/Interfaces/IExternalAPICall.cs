namespace PracticeWebApi.Services.Interfaces
{
    public interface IExternalAPICall
    {
        Task<string> GetCatPhotosAsync();
    }
}
