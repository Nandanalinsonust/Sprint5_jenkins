using HealthAxis.Api.Data;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.Api.Repositories.Impl
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly AppDbContext context;

        public AppointmentRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Appointment>> GetByDoctorIdAsync(int doctorId)
        {
            return await context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<Appointment?> UpdateStatusAsync(int id, string status)
        {
            var entity = await context.Appointments.FindAsync(id);
            if (entity == null) return null;

            entity.Status = status;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await context.Appointments.FindAsync(id);
            if (entity == null) return false;

            context.Appointments.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AppointmentSummary>> GetSummaryReportAsync()
        {
            return await context.Appointments
                .GroupBy(a => a.ScheduledDate.Date)
                .Select(g => new AppointmentSummary
                {
                    Date = g.Key,

                    Confirmed = g.Count(x => x.Status == AppointmentStatus.Confirmed.ToString()),
                    Cancelled = g.Count(x => x.Status == AppointmentStatus.Cancelled.ToString()),
                    Completed = g.Count(x => x.Status == AppointmentStatus.Completed.ToString())

                })
                .ToListAsync();
        }
    }
}