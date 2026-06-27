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

        // ✅ DAY / WEEK COUNTS
        public async Task<DashboardCountDto?> GetCountsAsync(string range)
        {
            return await GetAsync<DashboardCountDto>(
                $"api/admin/dashboard/counts?range={range}");
        }

        // ✅ DEPARTMENT GRAPH
        public async Task<List<DepartmentChartDto>> GetDepartmentStatsAsync(string range)
        {
            return await GetAsync<List<DepartmentChartDto>>(
                $"api/admin/dashboard/departments?range={range}");
        }

        // ✅ DOCTOR INSIDE DEPARTMENT PIE
        public async Task<List<DoctorChartDto>> GetDoctorByDepartment(string department)
        {
            return await GetAsync<List<DoctorChartDto>>(
                $"api/admin/dashboard/doctor-by-department?dept={department}");
        }
    }
}