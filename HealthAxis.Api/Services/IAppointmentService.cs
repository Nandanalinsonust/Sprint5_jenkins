using HealthAxis.Api.Models.Dtos;

namespace HealthAxis.Api.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();

        Task<AppointmentDto> GetByIdAsync(int id);

        Task<AppointmentDto> AddAsync(AppointmentDto entity);

        Task<AppointmentDto> UpdateAsync(int id, AppointmentDto entity);
    }
}
