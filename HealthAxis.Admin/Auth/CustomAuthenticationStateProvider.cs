using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace HealthAxis.Admin.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
        private readonly IJSRuntime _js;

        public CustomAuthenticationStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "accessToken");

            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(_currentUser);

            var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
            _currentUser = new ClaimsPrincipal(identity);

            return new AuthenticationState(_currentUser);
        }

        private IEnumerable<Claim> ParseClaims(string jwt)
        {
            var claims = new List<Claim>();

            var payload = jwt.Split('.')[1];
            payload = Pad(payload);

            var bytes = Convert.FromBase64String(payload);

            var kvp = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(bytes);

            if (kvp == null) return claims;

            foreach (var item in kvp)
            {
                if (item.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var val in item.Value.EnumerateArray())
                    {
                        claims.Add(new Claim(item.Key, val.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(item.Key, item.Value.ToString()));
                }
            }

            return claims;
        }

        private string Pad(string s)
        {
            return (s.Length % 4) switch
            {
                2 => s + "==",
                3 => s + "=",
                _ => s
            };
        }

        public void NotifyUserLoggedIn(string token)
        {
            var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
            _currentUser = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public void NotifyUserLoggedOut()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}