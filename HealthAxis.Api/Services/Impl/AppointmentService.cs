using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;

namespace HealthAxis.Api.Services.Impl
{
    public class AppointmentService(
        IAppointmentRepository repository,
        IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> AddAsync(AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);

            var saved = await repository.CreateAsync(appointment);

            return mapper.Map<AppointmentDto>(saved);
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var data = await repository.GetAllAsync();

            return mapper.Map<List<AppointmentDto>>(data);
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            var result = await repository.GetByIdAsync(id);

            return mapper.Map<AppointmentDto>(result);
        }

        public async Task<AppointmentDto> UpdateAsync(int id, AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);

            appointment.AppointmentId = id;

            var updated = await repository.UpdateAsync(id, appointment);

            return mapper.Map<AppointmentDto>(updated);
        }
    }
}