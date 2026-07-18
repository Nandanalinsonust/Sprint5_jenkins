using HealthAxis.Api.Data;
using HealthAxis.Api.Models;
using HealthAxis.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.Api.Repository.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly HealthAxisDbContext context;

        public PatientRepository(HealthAxisDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task<bool> IsDuplicatePatientAsync(
            string patientName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            int? excludePatientId = null,
            CancellationToken ct = default)
        {
            string normalizedPatientName = patientName.Trim().ToUpper();
            string normalizedEmail = email.Trim().ToUpper();

            return await context.Patients.AnyAsync(
                patient =>
                    patient.PatientName.ToUpper() == normalizedPatientName &&
                    patient.Email.ToUpper() == normalizedEmail &&
                    patient.PhoneNumber == phoneNumber &&
                    patient.DateOfBirth.Date == dateOfBirth.Date &&
                    (!excludePatientId.HasValue ||
                     patient.PatientId != excludePatientId.Value),
                ct);
        }

        public async Task<Patient?> GetByIdentityUserIdAsync(
            string identityUserId,
            CancellationToken ct = default)
        {
            return await context.Patients
                .FirstOrDefaultAsync(
                    patient => patient.IdentityUserId == identityUserId,
                    ct);
        }
    }
}