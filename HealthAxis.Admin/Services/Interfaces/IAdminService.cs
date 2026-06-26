using HealthAxis.Shared.Dtos;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardCountDto?> GetCountsAsync();

        Task<List<DoctorChartDto>> GetDoctorStatsAsync();

        Task<List<DepartmentChartDto>> GetDepartmentStatsAsync();
    }
}