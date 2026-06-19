using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IAuthService
    {
        Task<(bool Success,string Message,string UserId)>Register(RegisterDto request);
        Task<(bool Success, string Message, AuthResponse? Data, int ExpiresIn)> Login(LoginDto request);
        Task<(bool Success, string Message)> CreateDoctorUser(string email);

        Task<(bool Success, string Message)> ChangePassword(string email,string oldPassword,string newPassword);
    }
}
