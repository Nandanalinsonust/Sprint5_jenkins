using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IPatientService
    {
        Task<PatientDto> AddAsync(CreatePatientDto entity);

        Task<List<PatientDto>> GetAllAsync();

        Task<PatientDto> GetByIdAsync(int id);

        Task<PatientDto> UpdateAsync(int id, PatientDto entity);

        Task<List<PatientDto>> GetByNameAsync(string name);

        Task<bool> DeactivateAsync(int id);
    }
}