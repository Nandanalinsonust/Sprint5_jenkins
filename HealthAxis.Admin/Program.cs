using HealthAxis.Admin;
using HealthAxis.Admin.Auth;
using HealthAxis.Admin.Services.Impl;
using HealthAxis.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

var blazorBaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

var apiBaseAddress = new Uri(
    blazorBaseAddress.GetLeftPart(UriPartial.Authority) + "/");

builder.Services.AddScoped(_ =>
    new HttpClient
    {
        BaseAddress = apiBaseAddress
    });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
    serviceProvider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IDoctorAdminService, DoctorAdminService>();
builder.Services.AddScoped<IPatientAdminService, PatientAdminService>();
builder.Services.AddScoped<IAppointmentAdminService, AppointmentAdminService>();
builder.Services.AddScoped<IToastService, ToastService>();

await builder.Build().RunAsync();