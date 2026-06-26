using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HealthAxis.Admin.Services
{
    public abstract class ApiService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly NavigationManager _nav;

        private const string TokenKey = "accessToken";

        protected ApiService(HttpClient http, IJSRuntime js, NavigationManager nav)
        {
            _http = http;
            _js = js;
            _nav = nav;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                RedirectToLogin();
                throw new UnauthorizedAccessException();
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RedirectToLogin();
                throw new UnauthorizedAccessException();
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>() ?? default!;
        }

        private void RedirectToLogin()
        {
            _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _nav.NavigateTo("/login", true);
        }
    }
}
