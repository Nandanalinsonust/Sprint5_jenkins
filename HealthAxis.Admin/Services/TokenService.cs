using Microsoft.JSInterop;

namespace HealthAxis.Admin.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _js;

        public TokenService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SaveTokensAsync(string accessToken, string refreshToken)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "accessToken", accessToken);
            await _js.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", "accessToken");
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", "refreshToken");
        }

        public async Task ClearAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "accessToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
        }
    }
}