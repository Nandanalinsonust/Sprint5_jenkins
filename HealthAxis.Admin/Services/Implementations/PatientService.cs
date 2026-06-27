using HealthAxis.Shared.Dtos;
using HealthAxis.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HealthAxis.Admin.Services.Implementations
{
    public class PatientService : ApiService, IPatientService
    {
        public PatientService(HttpClient http, IJSRuntime js, NavigationManager nav)
            : base(http, js, nav)
        {
        }

        // ✅ GET ALL
        public async Task<List<PatientDto>> GetAllAsync()
        {
            return await GetAsync<List<PatientDto>>("api/admin/patients/list") ?? new();
        }

        // ✅ TOGGLE (Deactivate/Activate)
        public async Task<bool> DeactivateAsync(int id)
        {
            return await PutAsync($"api/admin/patients/deactivate/{id}");
        }

        // ✅ GET BY ID
        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            return await GetAsync<PatientDto>($"api/patient/{id}");
        }

        // ✅ ADD
        public async Task<PatientDto> AddAsync(CreatePatientDto dto)
        {
            return await PostAsync<CreatePatientDto, PatientDto>(
                "api/patient", dto) ?? new();
        }

        // ✅ SEARCH
        public async Task<List<PatientDto>> GetByNameAsync(string name)
        {
            return await GetAsync<List<PatientDto>>(
                $"api/patient/name/{name}") ?? new();
        }
    }
}