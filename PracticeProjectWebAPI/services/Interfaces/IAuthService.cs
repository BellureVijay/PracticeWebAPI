using Microsoft.AspNetCore.Identity.Data;

namespace PracticeProjectWebAPI.services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
        Task<string> ForgetPassword(string email);
        Task<bool> ResetPassword(string token,string newPassword);
    }
}
