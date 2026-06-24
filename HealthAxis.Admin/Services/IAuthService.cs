using HealthAxis.Shared.Dtos;

namespace HealthAxis.Admin.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> LoginAsync(LoginDto dto);

        Task LogoutAsync();

        Task InitializeAsync();
    }
}
