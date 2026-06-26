using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();

        Task<DoctorDto?> GetByIdAsync(int id);

        Task<DoctorDto> AddAsync(CreateDoctorDto dto);

        Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto);

        Task<bool> DeactivateAsync(int id);

        Task<List<DoctorDto>> GetBySpecialisationAsync(DoctorSpecialisation specialisation);

        Task<List<DoctorDto>> GetByNameAsync(string name);
    }
}