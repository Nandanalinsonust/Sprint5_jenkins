using HealthAxis.Shared.Dtos.Auth;
using HealthAxis.Shared.Dtos.Patients;

namespace HealthAxis.Api.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, int PatientId)> RegisterPatientAsync(PatientRegisterDto request);

        Task<(bool Success, string Message, string Token, int ExpiresIn, bool MustChangePassword)> Login(LoginDto request);

        Task<(bool Success, string Message)> ChangePasswordAsync(string userId, ChangePasswordDto request);
    }
}