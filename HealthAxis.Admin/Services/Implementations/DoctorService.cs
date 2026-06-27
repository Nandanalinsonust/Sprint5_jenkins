using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;
using HealthAxis.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HealthAxis.Admin.Services.Implementations
{
    public class DoctorService : ApiService, IDoctorService
    {
        public DoctorService(HttpClient http, IJSRuntime js, NavigationManager nav)
            : base(http, js, nav)
        {
        }

        // ✅ GET ALL DOCTORS
        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return await GetAsync<List<DoctorDto>>(
                "api/admin/doctors/list") ?? new();
        }

        // ✅ ADD DOCTOR
        public async Task<DoctorDto?> AddAsync(CreateDoctorDto dto)
        {
            return await PostAsync<CreateDoctorDto, DoctorDto>("api/admin/doctors", dto);
        }

        public async Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            return await PutAsync<UpdateDoctorDto, DoctorDto>(
                $"api/admin/doctors/{id}", dto);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            return await PutAsync($"api/admin/doctors/deactivate/{id}");
        }

        // ✅ GET BY ID
        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            return await GetAsync<DoctorDto>(
                $"api/admin/doctors/{id}");
        }

        // ✅ SEARCH BY NAME
        public async Task<List<DoctorDto>> GetByNameAsync(string name)
        {
            return await GetAsync<List<DoctorDto>>(
                $"api/admin/doctors/name/{name}") ?? new();
        }

        // ✅ FILTER BY SPECIALISATION
        public async Task<List<DoctorDto>> GetBySpecialisationAsync(DoctorSpecialisation spec)
        {
            return await GetAsync<List<DoctorDto>>(
                $"api/admin/doctors/specialisation/{spec}") ?? new();
        }
    }
}