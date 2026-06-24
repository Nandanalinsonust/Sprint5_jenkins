using HealthAxis.Admin.Auth;
using HealthAxis.Shared.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthAxis.Admin.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly TokenService _tokenService;
        private readonly CustomAuthenticationStateProvider _authProvider;

        public AuthService(HttpClient http,
            TokenService tokenService,
            CustomAuthenticationStateProvider authProvider)
        {
            _http = http;
            _tokenService = tokenService;
            _authProvider = authProvider;
        }

        public async Task<(bool Success, string Message)> LoginAsync(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return (false, "Invalid credentials");

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            var token = result?.Data?.Token;

            if (string.IsNullOrEmpty(token))
                return (false, "Token missing");

            await _tokenService.SaveTokensAsync(token, "");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            _authProvider.NotifyUserLoggedIn(token);

            return (true, "Login success");
        }

        public async Task InitializeAsync()
        {
            var token = await _tokenService.GetAccessTokenAsync();

            if (string.IsNullOrEmpty(token))
                return;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            _authProvider.NotifyUserLoggedIn(token);
        }

        public async Task LogoutAsync()
        {
            await _tokenService.ClearAsync();

            _http.DefaultRequestHeaders.Authorization = null;

            _authProvider.NotifyUserLoggedOut();
        }
    }
}