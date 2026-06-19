using AutoMapper;
using HealthAxis.Api.Models;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Repositories;
using HealthAxis.Api.Services.Impl;
using Moq;
using Xunit;

namespace HealthAxis.Tests.ServiceTests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _repoMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new DoctorService(_repoMock.Object, _mapperMock.Object);
        }

        private CreateDoctorDto GetCreateDto()
        {
            return new CreateDoctorDto
            {
                FullName = "Test Doctor",
                Specialisation = "Cardiologist",
                YearsOfExperience = 5,
                ConsultationFee = 500
            };
        }

        private UpdateDoctorDto GetUpdateDto(int id)
        {
            return new UpdateDoctorDto
            {
                DoctorId = id,
                FullName = "Updated Doctor",
                Specialisation = "Dermatologist",
                YearsOfExperience = 10,
                ConsultationFee = 800,
                IsActive = true
            };
        }

        private Doctor GetDoctorEntity(int id = 1)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Test Doctor",
                Specialisation = "Cardiologist",
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private DoctorDto GetDoctorDto(int id = 1)
        {
            return new DoctorDto
            {
                DoctorId = id,
                FullName = "Test Doctor",
                Specialisation = "Cardiologist",
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        [Fact]
        public async Task AddAsync_ShouldCreateDoctor()
        {
            var dto = GetCreateDto();
            var entity = GetDoctorEntity();

            _mapperMock.Setup(m => m.Map<Doctor>(dto)).Returns(entity);
            _repoMock.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<DoctorDto>(entity)).Returns(GetDoctorDto());

            var result = await _service.AddAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Test Doctor", result.FullName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnDoctors()
        {
            var doctors = new List<Doctor> { GetDoctorEntity() };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(doctors);
            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors))
                .Returns(new List<DoctorDto> { GetDoctorDto() });

            var result = await _service.GetAllAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDoctor()
        {
            var doctor = GetDoctorEntity();

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doctor);
            _mapperMock.Setup(m => m.Map<DoctorDto>(doctor))
                .Returns(GetDoctorDto());

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDoctor()
        {
            var dto = GetUpdateDto(1);
            var updated = GetDoctorEntity();

            _mapperMock.Setup(m => m.Map<Doctor>(dto)).Returns(updated);
            _repoMock.Setup(r => r.UpdateAsync(1, It.IsAny<Doctor>()))
                .ReturnsAsync(updated);
            _mapperMock.Setup(m => m.Map<DoctorDto>(updated))
                .Returns(GetDoctorDto());

            var result = await _service.UpdateAsync(1, dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorId);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnDoctors()
        {
            var list = new List<Doctor> { GetDoctorEntity() };

            _repoMock.Setup(r => r.GetByNameAsync("Test"))
                .ReturnsAsync(list);

            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(list))
                .Returns(new List<DoctorDto> { GetDoctorDto() });

            var result = await _service.GetByNameAsync("Test");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetBySpecialisationAsync_ShouldReturnDoctors()
        {
            var list = new List<Doctor> { GetDoctorEntity() };

            _repoMock.Setup(r => r.GetBySpecialisationAsync("Cardiologist"))
                .ReturnsAsync(list);

            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(list))
                .Returns(new List<DoctorDto> { GetDoctorDto() });

            var result = await _service.GetBySpecialisationAsync("Cardiologist");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAvailabilityAsync_ShouldReturnAvailability()
        {
            var expected = new List<string> { "10:00-11:00" };

            _repoMock.Setup(r => r.GetAvailabilityAsync(1, It.IsAny<DateTime>()))
                .ReturnsAsync(expected);

            var result = await _service.GetAvailabilityAsync(1, DateTime.Today);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenSuccess()
        {
            _repoMock.Setup(r => r.DeactivateAsync(1)).ReturnsAsync(true);

            var result = await _service.DeactivateAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldReturnFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.DeactivateAsync(1)).ReturnsAsync(false);

            var result = await _service.DeactivateAsync(1);

            Assert.False(result);
        }
    }
}