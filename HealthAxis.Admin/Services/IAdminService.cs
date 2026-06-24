using HealthAxis.Shared.Dtos;

namespace HealthAxis.Admin.Services
{
    public interface IAdminService
    {
        Task<List<DoctorDto>> GetDoctors(int page = 1, int pageSize = 10);
        Task<DoctorDto?> CreateDoctor(CreateDoctorDto dto);
        Task<bool> UpdateDoctor(int id, UpdateDoctorDto dto);
        Task<bool> DeactivateDoctor(int id);

        Task<List<PatientDto>> GetPatients(int page = 1, int pageSize = 10);
        Task<bool> DeactivatePatient(int id);

        Task<List<AppointmentSummaryDto>> GetAppointmentSummary(); // ✅ FIXED
    }
}
