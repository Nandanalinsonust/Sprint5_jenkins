using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;

namespace HealthAxis.Api.Services.Impl
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService
    {
        public async Task<DoctorDto> AddAsync(DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var saved = await repository.CreateAsync(doctor);
            return mapper.Map<DoctorDto>(saved);
        }

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            return mapper.Map<DoctorDto>(await repository.GetByIdAsync(id));
        }

        public async Task<DoctorDto> UpdateAsync(int id, DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            doctor.DoctorId = id;
            var updated = await repository.UpdateAsync(id, doctor);
            return mapper.Map<DoctorDto>(updated);
        }

        public async Task<List<DoctorDto>> GetByNameAsync(string name)
        {
            var data = await repository.GetByNameAsync(name);
            return mapper.Map<List<DoctorDto>>(data);
        }

        public async Task<List<DoctorDto>> GetBySpecialisationAsync(string specialization)
        {
            var data = await repository.GetBySpecialisationAsync(specialization);
            return mapper.Map<List<DoctorDto>>(data);
        }

        public async Task<object> GetAvailabilityAsync(int doctorId, DateTime date)
        {
            return await repository.GetAvailabilityAsync(doctorId, date);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            return await repository.DeactivateAsync(id);
        }
    }
}