using HealthAxis.Shared.Dtos;
using HealthAxis.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HealthAxis.Admin.Services.Implementations
{
    public class AdminService : ApiService, IAdminService
    {
        public AdminService(HttpClient http, IJSRuntime js, NavigationManager nav)
            : base(http, js, nav)
        {
        }

        public async Task<DashboardCountDto?> GetCountsAsync()
        {
            return await GetAsync<DashboardCountDto>("api/admin/dashboard/counts");
        }

        public async Task<List<DoctorChartDto>> GetDoctorStatsAsync()
        {
            return await GetAsync<List<DoctorChartDto>>("api/admin/dashboard/today-doctor");
        }

        public async Task<List<DepartmentChartDto>> GetDepartmentStatsAsync()
        {
            return await GetAsync<List<DepartmentChartDto>>("api/admin/dashboard/departments");
        }
    }
}