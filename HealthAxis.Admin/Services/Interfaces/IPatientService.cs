using HealthAxis.Shared.Dtos;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();

        Task<PatientDto?> GetByIdAsync(int id);

        Task<PatientDto> AddAsync(CreatePatientDto dto);

        Task<bool> DeactivateAsync(int id);

        Task<List<PatientDto>> GetByNameAsync(string name);
    }
}