using HealthAxis.Shared.Dtos.Auth;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        Task LogoutAsync();

        Task<bool> IsAuthenticatedAsync();

        Task<string?> GetCurrentUserEmailAsync();

        Task<string?> GetCurrentUserRoleAsync();
    }
}