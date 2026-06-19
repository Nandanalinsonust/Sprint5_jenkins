using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IDoctorService
    {
        Task<DoctorDto> AddAsync(CreateDoctorDto entity);
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto entity);
        Task<List<DoctorDto>> GetByNameAsync(string name);
        Task<List<DoctorDto>> GetBySpecialisationAsync(string specialization);
        Task<object> GetAvailabilityAsync(int doctorId, DateTime date);
        Task<bool> DeactivateAsync(int id);
    }
}