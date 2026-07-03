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
           .Include(h => h.Patient)
           .Include(h => h.Doctor)
           .Include(h => h.Appointment)
           .Where(h => h.PatientId == patientId)
           .ToListAsync();
        }

        public async Task<List<HealthRecord>> GetByDoctorIdAsync(int doctorId)
        {
            return await context.HealthRecords
            .Include(h => h.Patient)
            .Include(h => h.Doctor)
            .Include(h => h.Appointment)
            .Where(h => h.DoctorId == doctorId)
            .ToListAsync();
        }
    }
}