using HealthAxis.Shared.Dtos;
using System.Net.Http.Json;

namespace HealthAxis.Admin.Services.Impl
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _http;

        public AdminService(HttpClient http)
        {
            _http = http;
        }

        // ✅ DOCTORS

        public async Task<List<DoctorDto>> GetDoctors(int page = 1, int pageSize = 10)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<DoctorDto>>(
                    $"api/admin/doctors/list?page={page}&pageSize={pageSize}"
                ) ?? new();
            }
            catch
            {
                return new();
            }
        }

        public async Task<DoctorDto?> CreateDoctor(CreateDoctorDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/admin/doctors", dto);

            if (!res.IsSuccessStatusCode) return null;

            return await res.Content.ReadFromJsonAsync<DoctorDto>();
        }

        public async Task<bool> UpdateDoctor(int id, UpdateDoctorDto dto)
        {
            var res = await _http.PutAsJsonAsync($"api/admin/doctors/{id}", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeactivateDoctor(int id)
        {
            var res = await _http.PutAsync($"api/admin/doctors/deactivate/{id}", null);
            return res.IsSuccessStatusCode;
        }

        // ✅ PATIENTS

        public async Task<List<PatientDto>> GetPatients(int page = 1, int pageSize = 10)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<PatientDto>>(
                    $"api/admin/patients/list?page={page}&pageSize={pageSize}"
                ) ?? new();
            }
            catch
            {
                return new();
            }
        }

        public async Task<bool> DeactivatePatient(int id)
        {
            var res = await _http.PutAsync($"api/admin/patients/deactivate/{id}", null);
            return res.IsSuccessStatusCode;
        }

        // ✅ REPORTS (IMPORTANT FIX ✅)

        public async Task<List<AppointmentSummaryDto>> GetAppointmentSummary()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<AppointmentSummaryDto>>(
                    "api/admin/reports/appointments"
                ) ?? new();
            }
            catch
            {
                return new();
            }
        }
    }
}