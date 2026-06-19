using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;

namespace HealthAxis.Api.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository repository;
        private readonly IMapper mapper;

        public HealthRecordService(IHealthRecordRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<HealthRecordDto> AddAsync(CreateHealthRecordDto entity)
        {
            var record = mapper.Map<HealthRecord>(entity);
            record.VisitDate = DateTime.UtcNow;

            var saved = await repository.CreateAsync(record);

            return mapper.Map<HealthRecordDto>(saved);
        }

        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            var data = await repository.GetAllAsync();
            return mapper.Map<List<HealthRecordDto>>(data);
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            var result = await repository.GetByIdAsync(id);
            return mapper.Map<HealthRecordDto>(result);
        }

        public async Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId)
        {
            var data = await repository.GetByPatientIdAsync(patientId);
            return mapper.Map<List<HealthRecordDto>>(data);
        }

        public async Task<List<HealthRecordDto>> GetByDoctorIdAsync(int doctorId)
        {
            var data = await repository.GetByDoctorIdAsync(doctorId);
            return mapper.Map<List<HealthRecordDto>>(data);
        }
    }
}