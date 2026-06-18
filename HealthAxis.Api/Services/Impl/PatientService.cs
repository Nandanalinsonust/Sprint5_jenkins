using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;

namespace HealthAxis.Api.Services.Impl
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        public async Task<PatientDto> AddAsync(PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            var saved = await repository.CreateAsync(patient);
            return mapper.Map<PatientDto>(saved);
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            var data = await repository.GetAllAsync();
            return mapper.Map<List<PatientDto>>(data);
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            var result = await repository.GetByIdAsync(id);
            return mapper.Map<PatientDto>(result);
        }

        public async Task<PatientDto> UpdateAsync(int id, PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            patient.PatientId = id;

            var updated = await repository.UpdateAsync(id, patient);
            return mapper.Map<PatientDto>(updated);
        }

        public async Task<List<PatientDto>> GetByNameAsync(string name)
        {
            var data = await repository.GetByNameAsync(name);
            return mapper.Map<List<PatientDto>>(data);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            return await repository.DeactivateAsync(id);
        }
    }
}