using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;
using HealthAxis.Api.Services.Impl;
using Moq;
using Xunit;

namespace HealthAxis.Tests.ServiceTests
{
    public class HealthRecordServiceTests
    {
        private readonly Mock<IHealthRecordRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            _repoMock = new Mock<IHealthRecordRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new HealthRecordService(_repoMock.Object, _mapperMock.Object);
        }

        private CreateHealthRecordDto GetCreateDto()
        {
            return new CreateHealthRecordDto
            {
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 1,
                Diagnosis = "Flu",
                Prescription = "Rest and medication",
                Notes = "Take care"
            };
        }

        private HealthRecord GetEntity(int id = 1)
        {
            return new HealthRecord
            {
                HealthRecordId = id,
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 1,
                Diagnosis = "Flu",
                Prescription = "Rest and medication",
                Notes = "Take care",
                VisitDate = DateTime.UtcNow,

                Patient = new Patient
                {
                    PatientId = 1,
                    FullName = "Test Patient",
                    DateOfBirth = DateTime.Today.AddYears(-25),
                    Gender = "Male",
                    Email = "test@mail.com",
                    PhoneNumber = "1234567890",
                    InsuranceID = "INS123",
                    IsActive = true
                },

                Doctor = new Doctor
                {
                    DoctorId = 2,
                    FullName = "Test Doctor",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                },

                Appointment = new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 2,
                    ScheduledDate = DateTime.Today,
                    TimeSlot = "10:00-11:00",
                    Status = "Completed",
                    CancellationReason = null,

                    Patient = new Patient
                    {
                        PatientId = 1,
                        FullName = "Test Patient",
                        DateOfBirth = DateTime.Today.AddYears(-25),
                        Gender = "Male",
                        Email = "test@mail.com",
                        PhoneNumber = "1234567890",
                        InsuranceID = "INS123",
                        IsActive = true
                    },

                    Doctor = new Doctor
                    {
                        DoctorId = 2,
                        FullName = "Test Doctor",
                        Specialisation = "Cardiology",
                        YearsOfExperience = 5,
                        ConsultationFee = 500,
                        IsActive = true
                    }
                }
            };
        }

        private HealthRecordDto GetDto(int id = 1)
        {
            return new HealthRecordDto
            {
                HealthRecordId = id,
                PatientId = 1,
                DoctorId = 2,
                AppointmentId = 1,
                Diagnosis = "Flu",
                Prescription = "Rest and medication",
                Notes = "Take care",
                VisitDate = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task AddAsync_ShouldCreateHealthRecord()
        {
            var dto = GetCreateDto();
            var entity = GetEntity();

            _mapperMock.Setup(m => m.Map<HealthRecord>(dto)).Returns(entity);
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<HealthRecord>())).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<HealthRecordDto>(entity)).Returns(GetDto());

            var result = await _service.AddAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Flu", result.Diagnosis);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnRecords()
        {
            var list = new List<HealthRecord> { GetEntity() };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list))
                .Returns(new List<HealthRecordDto> { GetDto() });

            var result = await _service.GetAllAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecord()
        {
            var entity = GetEntity();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<HealthRecordDto>(entity)).Returns(GetDto());

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.HealthRecordId);
        }

        [Fact]
        public async Task GetByPatientIdAsync_ShouldReturnRecords()
        {
            var list = new List<HealthRecord> { GetEntity() };

            _repoMock.Setup(r => r.GetByPatientIdAsync(1)).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list))
                .Returns(new List<HealthRecordDto> { GetDto() });

            var result = await _service.GetByPatientIdAsync(1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByDoctorIdAsync_ShouldReturnRecords()
        {
            var list = new List<HealthRecord> { GetEntity() };

            _repoMock.Setup(r => r.GetByDoctorIdAsync(2)).ReturnsAsync(list);
            _mapperMock.Setup(m => m.Map<List<HealthRecordDto>>(list))
                .Returns(new List<HealthRecordDto> { GetDto() });

            var result = await _service.GetByDoctorIdAsync(2);

            Assert.Single(result);
        }
    }
}