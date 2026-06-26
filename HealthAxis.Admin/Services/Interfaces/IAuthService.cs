using HealthAxis.Shared.Dtos;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> LoginAsync(LoginDto dto);

        Task LogoutAsync();

        Task<bool> IsAuthenticatedAsync();

        Task<string?> GetCurrentUserRoleAsync();

        Task<string?> GetCurrentUserEmailAsync();
    }
}