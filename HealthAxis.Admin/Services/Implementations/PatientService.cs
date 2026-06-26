using HealthAxis.Shared.Dtos;
using System.Net.Http.Json;
using HealthAxis.Admin.Services.Interfaces;

namespace HealthAxis.Admin.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _http;

        public PatientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<PatientDto>>(
                "api/admin/patients/list") ?? new();
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var res = await _http.PutAsync($"api/admin/patients/deactivate/{id}", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<PatientDto>($"api/patient/{id}");
        }

        public async Task<PatientDto> AddAsync(CreatePatientDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/patient", dto);
            return await res.Content.ReadFromJsonAsync<PatientDto>();
        }

        public async Task<List<PatientDto>> GetByNameAsync(string name)
        {
            return await _http.GetFromJsonAsync<List<PatientDto>>(
                $"api/patient/name/{name}") ?? new();
        }
    }
}
