using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetAllAsync();

        Task<HealthRecordDto> GetByIdAsync(int id);

        Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId);

        Task<HealthRecordDto> AddAsync(HealthRecordDto entity);
    }
}