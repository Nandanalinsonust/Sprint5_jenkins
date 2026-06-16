using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();

        Task<DoctorDto> GetByIdAsync(int id);

        Task<DoctorDto> AddAsync(DoctorDto entity);
        Task<DoctorDto> UpdateAsync(int id, DoctorDto entity);
    }
}
