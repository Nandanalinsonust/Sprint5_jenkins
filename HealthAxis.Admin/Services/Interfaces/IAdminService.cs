using HealthAxis.Shared.Dtos;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardCountDto?> GetCountsAsync(string range);

        Task<List<DoctorChartDto>> GetDoctorByDepartment(string department);

        Task<List<DepartmentChartDto>> GetDepartmentStatsAsync(string range);
    }
}