using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<List<Patient>> GetByNameAsync(string name);
        Task<bool> DeactivateAsync(int id);
        Task<Patient?> GetByUserIdAsync(string userId);
    }
}