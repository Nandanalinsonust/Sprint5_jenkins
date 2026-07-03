using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repository.Interface
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<bool> IsDuplicatePatientAsync(
            string patientName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            int? excludePatientId = null,
            CancellationToken ct = default);

        Task<Patient?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken ct = default);
    }
}