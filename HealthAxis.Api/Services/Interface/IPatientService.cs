using HealthAxis.Shared.Dtos.Auth;
using HealthAxis.Shared.Dtos.Pagination;
using HealthAxis.Shared.Dtos.Patients;

namespace HealthAxis.Api.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PagedResponse<PatientDto>> GetAllPatientsPagedAsync(PatientPaginationQueryDto query);


        Task<PatientDto> GetPatientByIdAsync(int patientId);

        Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto);

        Task<PatientDto> UpdatePatientAsync(int patientId, UpdatePatientDto dto);

        Task<PatientDto> GetMyProfileAsync(string identityUserId);

        Task<PatientDto> UpdateMyProfileAsync(string identityUserId, UpdatePatientDto dto);


    }
}