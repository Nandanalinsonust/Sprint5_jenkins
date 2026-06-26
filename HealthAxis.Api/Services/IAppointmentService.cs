using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Api.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> AddAsync(CreateAppointmentDto entity);
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto entity);
        Task<List<AppointmentDto>> GetByDoctorIdAsync(int doctorId);
        Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId);
        Task<AppointmentDto> UpdateStatusAsync(int id, AppointmentStatus status);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasDoctorTreatedPatient(int doctorId, int patientId);
        Task<bool> CancelWithValidation(int id);
        Task<List<AppointmentSummary>> GetSummaryReportAsync();

    }
}
