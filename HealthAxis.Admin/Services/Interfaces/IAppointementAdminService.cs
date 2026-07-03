using HealthAxis.Shared.Dtos.Appointments;
using HealthAxis.Shared.Dtos.Pagination;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IAppointmentAdminService
    {
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();

        Task<PagedResponse<AppointmentDto>> GetAppointmentsPagedAsync(AppointmentPaginationQueryDto query);

        Task<AppointmentFilterOptionsDto> GetAppointmentFilterOptionsAsync();

        Task<AppointmentDailyStatusSummaryDto> GetDailyStatusSummaryAsync(DateTime date);


        Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId);

        Task<AppointmentDto> CreateAppointmentAsync(BookAppointmentDto request);

        Task<AppointmentDto?> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto request);

        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}