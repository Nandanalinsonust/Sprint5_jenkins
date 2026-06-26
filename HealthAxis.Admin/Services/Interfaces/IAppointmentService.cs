using HealthAxis.Shared.Dtos;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();

        Task<AppointmentDto?> GetByIdAsync(int id);

        Task<List<AppointmentSummaryDto>> GetSummaryReportAsync();

        Task<List<AppointmentDto>> GetByDoctorIdAsync(int doctorId);

        Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId);
    }
}
