using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HealthRecordController(
        IHealthRecordService service,
        IPatientService patientService,
        IDoctorService doctorService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create(CreateHealthRecordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var doctor = await doctorService.GetByUserIdAsync(userId);

            dto.DoctorId = doctor.DoctorId;

            return Ok(await service.AddAsync(dto));
        }

        [HttpGet("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyRecords()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = await patientService.GetByUserIdAsync(userId);

            return Ok(await service.GetByPatientIdAsync(patient.PatientId));
        }

        [HttpGet("doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorRecords()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var doctor = await doctorService.GetByUserIdAsync(userId);

            var records = await service.GetAllAsync();

            var filtered = records.Where(x =>
                x.DoctorId == doctor.DoctorId ||
                service.GetByPatientIdAsync(x.PatientId).Result.Any()).ToList();

            return Ok(filtered);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await service.GetByIdAsync(id);

            if (record == null) return NotFound();

            if (User.IsInRole("Patient"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var patient = await patientService.GetByUserIdAsync(userId);

                if (record.PatientId != patient.PatientId)
                    return Unauthorized();
            }

            return Ok(record);
        }
    }
}