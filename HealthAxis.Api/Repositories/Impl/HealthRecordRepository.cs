using HealthAxis.Api.Data;
using HealthAxis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.Api.Repositories.Impl
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        private readonly AppDbContext context;

        public HealthRecordRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<HealthRecord>> GetByPatientIdAsync(int patientId)
        {
            return await context.HealthRecords
                .Include(hr => hr.Doctor)
                .Include(hr => hr.Patient)
                .Where(hr => hr.PatientId == patientId)
                .ToListAsync();
        }
    }
}