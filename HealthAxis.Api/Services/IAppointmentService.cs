using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> AddAsync(AppointmentDto entity);
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<AppointmentDto> UpdateAsync(int id, AppointmentDto entity);
        Task<List<AppointmentDto>> GetByDoctorIdAsync(int doctorId);
        Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId);
        Task<AppointmentDto> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
        Task<object> GetSummaryReportAsync();
    }
}
