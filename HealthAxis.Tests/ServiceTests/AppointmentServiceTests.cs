using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Repositories;
using HealthAxis.Api.Services.Impl;
using Moq;
using Xunit;
using HealthAxis.Api.Exceptions;

namespace HealthAxis.Tests.ServiceTests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _repoMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new AppointmentService(_repoMock.Object, _mapperMock.Object);
        }

        private CreateAppointmentDto GetValidDto()
        {
            return new CreateAppointmentDto
            {
                PatientId = 1,
                DoctorId = 2,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00-11:00"
            };
        }

        private Appointment CreateValidAppointment(CreateAppointmentDto dto)
        {
            return new Appointment
            {
                AppointmentId = 1,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = "Pending",
                CancellationReason = null,

                Patient = new Patient
                {
                    PatientId = dto.PatientId,
                    FullName = "Test Patient",
                    DateOfBirth = DateTime.Today.AddYears(-25),
                    Gender = "Male",
                    Email = "test@mail.com",
                    PhoneNumber = "1234567890",
                    InsuranceID = "INS1234",
                    IsActive = true
                },

                Doctor = new Doctor
                {
                    DoctorId = dto.DoctorId,
                    FullName = "Test Doctor",
                    Specialisation = "Cardiology",
                    YearsOfExperience = 5,
                    ConsultationFee = 500,
                    IsActive = true
                }
            };
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenPastDate()
        {
            var dto = GetValidDto();
            dto.ScheduledDate = DateTime.Today.AddDays(-1);

            await Assert.ThrowsAsync<InvalidException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenPastTimeToday()
        {
            var dto = GetValidDto();
            dto.ScheduledDate = DateTime.Today;
            dto.TimeSlot = "01:00-02:00";

            await Assert.ThrowsAsync<InvalidException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenSameDoctorSameDay()
        {
            var dto = GetValidDto();

            _repoMock.Setup(r => r.GetByPatientIdAsync(dto.PatientId))
                .ReturnsAsync(new List<Appointment>
                {
                    CreateValidAppointment(dto)
                });

            await Assert.ThrowsAsync<InvalidException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenSameTimeSlot()
        {
            var dto = GetValidDto();

            var existing = CreateValidAppointment(dto);

            _repoMock.Setup(r => r.GetByPatientIdAsync(dto.PatientId))
                .ReturnsAsync(new List<Appointment> { existing });

            await Assert.ThrowsAsync<InvalidException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenDoctorNotAvailable()
        {
            var dto = GetValidDto();

            _repoMock.Setup(r => r.GetByPatientIdAsync(dto.PatientId))
                .ReturnsAsync(new List<Appointment>());

            _repoMock.Setup(r => r.GetByDoctorIdAsync(dto.DoctorId))
                .ReturnsAsync(new List<Appointment>
                {
                    CreateValidAppointment(dto)
                });

            await Assert.ThrowsAsync<InvalidException>(() => _service.AddAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ShouldCreateAppointment_WhenValid()
        {
            var dto = GetValidDto();
            var appointment = CreateValidAppointment(dto);

            _repoMock.Setup(r => r.GetByPatientIdAsync(dto.PatientId))
                .ReturnsAsync(new List<Appointment>());

            _repoMock.Setup(r => r.GetByDoctorIdAsync(dto.DoctorId))
                .ReturnsAsync(new List<Appointment>());

            _mapperMock.Setup(m => m.Map<Appointment>(dto))
                .Returns(appointment);

            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Appointment>()))
                .ReturnsAsync(appointment);

            _mapperMock.Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(new AppointmentDto
                {
                    AppointmentId = 1
                });

            var result = await _service.AddAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }
    }
}