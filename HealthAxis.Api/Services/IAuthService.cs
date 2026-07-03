using HealthAxis.Shared.Dtos;
using System.Security.Claims;

namespace HealthAxis.Api.Services
{
    public interface IAuthService
    {
        Task<(bool Success,string Message,string UserId)>Register(RegisterDto request);
        Task<(bool Success, string Message, AuthResponse? Data, int ExpiresIn)> Login(LoginDto request);
        Task<(bool Success, string Message)> CreateDoctorUser(string email);

        Task<(bool Success, string Message)> ChangePassword(
    ClaimsPrincipal principal,
    string currentPassword,
    string newPassword);
        Task<string> ForgotPassword(string email);
        Task<(bool Success, string Message)> CreatePatientUser(string email);
    }
}
