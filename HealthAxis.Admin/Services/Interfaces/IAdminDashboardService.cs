using HealthAxis.Shared.Dtos.Dashboard;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardReportDto> GetDashboardReportAsync();
    }
}