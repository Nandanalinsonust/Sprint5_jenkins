using HealthAxis.Api.Data;
using HealthAxis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.Api.Repositories.Impl
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly AppDbContext context;

        public DoctorRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Doctor>> GetByNameAsync(string name)
        {
            return await context.Doctors
                .Where(d => d.FullName.Contains(name))
                .ToListAsync();
        }

        public async Task<List<Doctor>> GetBySpecialisationAsync(string specialization)
        {
            return await context.Doctors
                .Where(d => d.Specialisation == specialization)
                .ToListAsync();
        }

        public async Task<object> GetAvailabilityAsync(int doctorId, DateTime date)
        {
            var bookedSlots = await context.Appointments
                .Where(a => a.DoctorId == doctorId && a.ScheduledDate.Date == date.Date)
                .Select(a => a.TimeSlot)
                .ToListAsync();

            return bookedSlots;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var doctor = await context.Doctors.FindAsync(id);
            if (doctor == null) return false;

            doctor.IsActive = false;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
