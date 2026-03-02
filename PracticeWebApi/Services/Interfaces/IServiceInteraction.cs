namespace PracticeWebApi.Services.Interfaces
{
    public interface IServiceInteraction
    {
        Task<string> CallOtherService();
    }
}
