using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace HealthAxis.Admin.Services
{
    public abstract class ApiService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly NavigationManager _nav;

        private const string TokenKey = "accessToken";
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
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

            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions) ?? default!;
        }
        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                RedirectToLogin();
                throw new UnauthorizedAccessException();
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(data);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RedirectToLogin();
                throw new UnauthorizedAccessException();
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }
        protected async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                RedirectToLogin();
                throw new UnauthorizedAccessException();
            }

            using var request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(data);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RedirectToLogin();
                throw new UnauthorizedAccessException();
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        protected async Task<bool> PutAsync(string url)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                RedirectToLogin();
                return false;
            }

            using var request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                RedirectToLogin();
                return false;
            }

            return response.IsSuccessStatusCode;
        }
        private void RedirectToLogin()
{
    _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);

    _nav.NavigateTo(
        "https://localhost:4200/login",
        true);
}
    }
}
