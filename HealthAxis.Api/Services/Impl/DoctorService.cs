using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;

namespace HealthAxis.Api.Services.Impl
{
    public class DoctorService(IDoctorRepository repository, IMapper mapper) : IDoctorService //primary constructor
    {
        public async Task<DoctorDto> AddAsync(DoctorDto entity)
        {
            var doctor = mapper.Map<Doctor>(entity);
            var savedEntity = await repository.CreateAsync(doctor);
            return mapper.Map<DoctorDto>(savedEntity);
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
            var updated= await repository.UpdateAsync(id , doctor);
            return mapper.Map<DoctorDto>(updated);
        }
    }
}