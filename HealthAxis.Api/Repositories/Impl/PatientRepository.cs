using HealthAxis.Api.Data;
using HealthAxis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.Api.Repositories.Impl
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly AppDbContext context;

        public PatientRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Patient>> GetByNameAsync(string name)
        {
            return await context.Patients
                .Where(p => p.FullName.Contains(name))
                .ToListAsync();
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient == null) return false;

            patient.IsActive = false;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<Patient?> GetByUserIdAsync(string userId)
        {
            return await context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}
