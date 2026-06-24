using HealthAxis.Admin;
using HealthAxis.Admin.Auth;
using HealthAxis.Admin.Services;
using HealthAxis.Admin.Services.Impl;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7273/")
});

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7273/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp=>
{
    return sp.GetRequiredService<CustomAuthenticationStateProvider>();
});


builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();


await builder.Build().RunAsync();
