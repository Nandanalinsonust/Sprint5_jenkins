using HealthAxis.Api.Models;

namespace HealthAxis.Api.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task<List<Appointment>> GetByPatientIdAsync(int patientId);
        Task<Appointment?> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
        Task<object> GetSummaryReportAsync();
    }
}