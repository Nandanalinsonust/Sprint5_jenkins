using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<List<Doctor>> GetByNameAsync(string name);
        Task<List<Doctor>> GetBySpecialisationAsync(string specialization);
        Task<object> GetAvailabilityAsync(int doctorId, DateTime date);
        Task<bool> DeactivateAsync(int id);
    }
}