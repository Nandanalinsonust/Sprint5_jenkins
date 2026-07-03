using HealthAxis.Shared.Dtos.Doctors;
using HealthAxis.Shared.Dtos.Pagination;

namespace HealthAxis.Admin.Services.Interfaces
{
    public interface IDoctorAdminService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<PagedResponse<DoctorDto>> GetDoctorsPagedAsync(DoctorPaginationQueryDto query);

        Task<DoctorDto?> GetDoctorByIdAsync(int doctorId);

        Task<DoctorCreatedResponseDto> CreateDoctorAsync(CreateDoctorDto doctorDto);

        Task<DoctorDto?> UpdateDoctorAsync(int doctorId, UpdateDoctorDto doctorDto);

        Task<DoctorDto?> ToggleDoctorStatusAsync(int doctorId);

    }
}