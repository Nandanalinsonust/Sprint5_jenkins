using HealthAxis.Shared.Dtos;
using HealthAxis.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HealthAxis.Admin.Services.Implementations
{
    public class AppointmentService : ApiService, IAppointmentService
    {
        public AppointmentService(HttpClient http, IJSRuntime js, NavigationManager nav)
            : base(http, js, nav)
        {
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return await GetAsync<List<AppointmentDto>>("api/admin/appointments/list") ?? new();
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            return await GetAsync<AppointmentDto>($"api/appointment/{id}");
        }

        public async Task<List<AppointmentSummaryDto>> GetSummaryReportAsync()
        {
            return await GetAsync<List<AppointmentSummaryDto>>(
                "api/reports/appointments") ?? new();
        }

        public async Task<List<AppointmentDto>> GetByDoctorIdAsync(int doctorId)
        {
            return await GetAsync<List<AppointmentDto>>(
                $"api/appointment/doctor?doctorId={doctorId}") ?? new();
        }

        public async Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            return await GetAsync<List<AppointmentDto>>(
                $"api/appointment/patient?patientId={patientId}") ?? new();
        }
    }
}