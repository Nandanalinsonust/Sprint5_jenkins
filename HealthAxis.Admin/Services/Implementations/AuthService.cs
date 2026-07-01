using HealthAxis.Admin.Auth;
using HealthAxis.Admin.Services.Interfaces;
using HealthAxis.Shared.Dtos;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace HealthAxis.Admin.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private const string TokenStorageKey = "accessToken"; // ✅ your key
        private const string AdminRoleName = "Admin";
        private const string LoginEndpoint = "api/auth/login";

        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly CustomAuthenticationStateProvider _authProvider;

        public AuthService(
            HttpClient http,
            IJSRuntime js,
            CustomAuthenticationStateProvider authProvider)
        {
            _http = http;
            _js = js;
            _authProvider = authProvider;
        }

        // ✅ LOGIN
        public async Task<(bool Success, string Message)> LoginAsync(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync(LoginEndpoint, dto);

            if (!response.IsSuccessStatusCode)
                return (false, "Invalid credentials");

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            var token = result?.Data?.Token;

            if (string.IsNullOrWhiteSpace(token))
                return (false, "Token missing");

            // ✅ Role validation (same as your friend)
            var role = GetRoleFromToken(token);

            if (!string.Equals(role, AdminRoleName, StringComparison.OrdinalIgnoreCase))
            {
                await LogoutAsync();
                return (false, "Only Admin users allowed");
            }

            // ✅ Save token
            await _js.InvokeVoidAsync("localStorage.setItem", TokenStorageKey, token);

            // ✅ Update auth state
            _authProvider.NotifyUserLoggedIn(token);

            return (true, "Login success");
        }

        // ✅ LOGOUT
        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenStorageKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", "token");

            _authProvider.NotifyUserLoggedOut();
        }

        // ✅ OPTIONAL HELPERS

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task<string?> GetCurrentUserRoleAsync()
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
                return null;

            return GetRoleFromToken(token);
        }

        public async Task<string?> GetCurrentUserEmailAsync()
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
                return null;

            return GetEmailFromToken(token);
        }

        // ✅ INTERNAL METHODS

        private async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>(
                "localStorage.getItem",
                TokenStorageKey);
        }

        private static string GetRoleFromToken(string token)
        {
            var payload = GetJwtPayload(token);

            const string roleClaimUri =
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

            if (payload.TryGetProperty(roleClaimUri, out var roleClaim))
                return roleClaim.GetString() ?? string.Empty;

            if (payload.TryGetProperty(ClaimTypes.Role, out var claimRole))
                return claimRole.GetString() ?? string.Empty;

            if (payload.TryGetProperty("role", out var simpleRole))
                return simpleRole.GetString() ?? string.Empty;

            return string.Empty;
        }

        private static string GetEmailFromToken(string token)
        {
            var payload = GetJwtPayload(token);

            if (payload.TryGetProperty("email", out var emailClaim))
                return emailClaim.GetString() ?? string.Empty;

            return string.Empty;
        }

        private static JsonElement GetJwtPayload(string token)
        {
            var parts = token.Split('.');

            if (parts.Length < 2)
                return default;

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            payload = Pad(payload);

            var bytes = Convert.FromBase64String(payload);

            var json = Encoding.UTF8.GetString(bytes);

            return JsonSerializer.Deserialize<JsonElement>(json);
        }

        private static string Pad(string base64)
        {
            return (base64.Length % 4) switch
            {
                2 => base64 + "==",
                3 => base64 + "=",
                _ => base64
            };
        }
    }
}
