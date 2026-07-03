//using AutoMapper;
//using HealthAxis.Api.Models;
//using HealthAxis.Shared.Dtos;
//using HealthAxis.Api.Repositories;
//using HealthAxis.Api.Services.Impl;
//using Moq;
//using Xunit;

//namespace HealthAxis.Tests.ServiceTests
//{
//    public class PatientServiceTests
//    {
//        private readonly Mock<IPatientRepository> _repoMock;
//        private readonly Mock<IMapper> _mapperMock;
//        private readonly PatientService _service;

//        public PatientServiceTests()
//        {
//            _repoMock = new Mock<IPatientRepository>();
//            _mapperMock = new Mock<IMapper>();

//            _service = new PatientService(_repoMock.Object, _mapperMock.Object);
//        }

//        private CreatePatientDto GetCreateDto()
//        {
//            return new CreatePatientDto
//            {
//                FullName = "Test Patient",
//                DateOfBirth = DateTime.Today.AddYears(-25),
//                Gender = "Male",
//                Email = "test@mail.com",
//                PhoneNumber = "1234567890",
//                InsuranceID = "INS123"
//            };
//        }

//        private PatientDto GetPatientDto(int id = 1)
//        {
//            return new PatientDto
//            {
//                PatientId = id,
//                FullName = "Test Patient",
//                DateOfBirth = DateTime.Today.AddYears(-25),
//                Gender = "Male",
//                Email = "test@mail.com",
//                PhoneNumber = "1234567890",
//                InsuranceID = "INS123",
//                IsActive = true
//            };
//        }

//        private Patient GetPatientEntity(int id = 1)
//        {
//            return new Patient
//            {
//                PatientId = id,
//                FullName = "Test Patient",
//                DateOfBirth = DateTime.Today.AddYears(-25),
//                Gender = "Male",
//                Email = "test@mail.com",
//                PhoneNumber = "1234567890",
//                InsuranceID = "INS123",
//                IsActive = true
//            };
//        }

//        [Fact]
//        public async Task AddAsync_ShouldCreatePatient()
//        {
//            var dto = GetCreateDto();
//            var entity = GetPatientEntity();

//            _mapperMock.Setup(m => m.Map<Patient>(dto)).Returns(entity);
//            _repoMock.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
//            _mapperMock.Setup(m => m.Map<PatientDto>(entity)).Returns(GetPatientDto());

//            var result = await _service.AddAsync(dto);

//            Assert.NotNull(result);
//            Assert.Equal("Test Patient", result.FullName);
//        }

//        [Fact]
//        public async Task GetAllAsync_ShouldReturnPatients()
//        {
//            var list = new List<Patient> { GetPatientEntity() };

//            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
//            _mapperMock.Setup(m => m.Map<List<PatientDto>>(list))
//                .Returns(new List<PatientDto> { GetPatientDto() });

//            var result = await _service.GetAllAsync();

//            Assert.Single(result);
//        }

//        [Fact]
//        public async Task GetByIdAsync_ShouldReturnPatient()
//        {
//            var entity = GetPatientEntity();

//            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
//            _mapperMock.Setup(m => m.Map<PatientDto>(entity))
//                .Returns(GetPatientDto());

//            var result = await _service.GetByIdAsync(1);

//            Assert.NotNull(result);
//            Assert.Equal(1, result.PatientId);
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldUpdatePatient()
//        {
//            var dto = GetPatientDto(1);
//            var entity = GetPatientEntity(1);

//            _mapperMock.Setup(m => m.Map<Patient>(dto)).Returns(entity);
//            _repoMock.Setup(r => r.UpdateAsync(1, It.IsAny<Patient>()))
//                .ReturnsAsync(entity);
//            _mapperMock.Setup(m => m.Map<PatientDto>(entity))
//                .Returns(GetPatientDto());

//            var result = await _service.UpdateAsync(1, dto);

//            Assert.NotNull(result);
//            Assert.Equal(1, result.PatientId);
//        }

//        [Fact]
//        public async Task GetByNameAsync_ShouldReturnPatients()
//        {
//            var list = new List<Patient> { GetPatientEntity() };

//            _repoMock.Setup(r => r.GetByNameAsync("Test"))
//                .ReturnsAsync(list);

//            _mapperMock.Setup(m => m.Map<List<PatientDto>>(list))
//                .Returns(new List<PatientDto> { GetPatientDto() });

//            var result = await _service.GetByNameAsync("Test");

//            Assert.Single(result);
//        }

//        [Fact]
//        public async Task DeactivateAsync_ShouldReturnTrue_WhenSuccess()
//        {
//            _repoMock.Setup(r => r.DeactivateAsync(1)).ReturnsAsync(true);

//            var result = await _service.DeactivateAsync(1);

//            Assert.True(result);
//        }

//        [Fact]
//        public async Task DeactivateAsync_ShouldReturnFalse_WhenNotFound()
//        {
//            _repoMock.Setup(r => r.DeactivateAsync(1)).ReturnsAsync(false);

//            var result = await _service.DeactivateAsync(1);

//            Assert.False(result);
//        }
//    }
//}
