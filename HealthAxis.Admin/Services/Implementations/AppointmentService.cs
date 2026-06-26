using HealthAxis.Shared.Dtos;
using System.Net.Http.Json;
using HealthAxis.Admin.Services.Interfaces;

namespace HealthAxis.Admin.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _http;

        public AppointmentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<AppointmentDto>>(
                "api/appointment") ?? new();
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<AppointmentDto>(
                $"api/appointment/{id}");
        }

        public async Task<List<AppointmentSummaryDto>> GetSummaryReportAsync()
        {
            return await _http.GetFromJsonAsync<List<AppointmentSummaryDto>>(
                "api/admin/reports/appointments") ?? new();
        }

        public async Task<List<AppointmentDto>> GetByDoctorIdAsync(int doctorId)
        {
            return await _http.GetFromJsonAsync<List<AppointmentDto>>(
                $"api/appointment/doctor?doctorId={doctorId}") ?? new();
        }

        public async Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            return await _http.GetFromJsonAsync<List<AppointmentDto>>(
                $"api/appointment/patient?patientId={patientId}") ?? new();
        }
    }
}