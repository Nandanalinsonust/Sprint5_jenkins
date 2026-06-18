using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repositories
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {
        Task<List<HealthRecord>> GetByPatientIdAsync(int patientId);
        Task<List<HealthRecord>> GetByDoctorIdAsync(int doctorId);
    }
}