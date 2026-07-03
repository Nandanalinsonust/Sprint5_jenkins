using HealthAxis.Shared.Dtos.Pagination;
using HealthAxis.Shared.Dtos.Patients;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IPatientAdminService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PagedResponse<PatientDto>> GetPatientsPagedAsync(PatientPaginationQueryDto query);

        Task<PatientDto?> GetPatientByIdAsync(int patientId);

        Task<PatientDto?> CreatePatientAsync(CreatePatientDto patientDto);

        Task<PatientDto?> UpdatePatientAsync(int patientId, UpdatePatientDto patientDto);
    }
}