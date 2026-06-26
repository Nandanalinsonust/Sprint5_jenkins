using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;
using System.Net.Http.Json;
using HealthAxis.Admin.Services.Interfaces;

namespace HealthAxis.Admin.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly HttpClient _http;

        public DoctorService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<DoctorDto>>(
                "api/admin/doctors/list") ?? new();
        }

        public async Task<DoctorDto?> AddAsync(CreateDoctorDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/admin/doctors", dto);
            return await res.Content.ReadFromJsonAsync<DoctorDto>();
        }

        public async Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var res = await _http.PutAsJsonAsync($"api/admin/doctors/{id}", dto);
            return await res.Content.ReadFromJsonAsync<DoctorDto>();
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var res = await _http.PutAsync($"api/admin/doctors/deactivate/{id}", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<DoctorDto>($"api/doctors/{id}");
        }

        public async Task<List<DoctorDto>> GetByNameAsync(string name)
        {
            return await _http.GetFromJsonAsync<List<DoctorDto>>(
                $"api/doctors/name/{name}") ?? new();
        }

        public async Task<List<DoctorDto>> GetBySpecialisationAsync(DoctorSpecialisation spec)
        {
            return await _http.GetFromJsonAsync<List<DoctorDto>>(
                $"api/doctors/specialisation/{spec}") ?? new();
        }
    }
}