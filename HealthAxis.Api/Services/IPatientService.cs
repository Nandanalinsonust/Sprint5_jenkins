using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();

        Task<PatientDto> GetByIdAsync(int id);

        Task<PatientDto> AddAsync(PatientDto entity);

        Task<PatientDto> UpdateAsync(int id, PatientDto entity);
    }
}