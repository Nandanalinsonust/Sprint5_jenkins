using HealthAxis.Api.Services;
using HealthAxis.Api.Services.Impl;
using HealthAxis.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController(
        IPatientService patientService,
        IAppointmentService appointmentService,
        IDoctorService doctorService, IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePatient(CreatePatientDto dto)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                    return Unauthorized();

                var existing = await patientService.GetByUserIdAsync(userId);
                if (existing != null)
                    return BadRequest("Patient profile already exists.");

                dto.UserId = userId;
                var patient = await patientService.AddAsync(dto);
                return Ok(patient);
            }

            var patient = await patientService.AddAsync(dto);

            var result = await authService.CreatePatientUser(dto.Email);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(patient);
        }

        [HttpPost("complete-profile")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CompleteProfile(CreatePatientDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var existing = await patientService.GetByUserIdAsync(userId);
            if (existing != null)
                return BadRequest("Profile already completed.");

            dto.UserId = userId;
            var patient = await patientService.AddAsync(dto);
            return Ok(patient);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Patient"))
            {
                var patient = await patientService.GetByUserIdAsync(userId);

                if (patient.PatientId != id)
                    return Unauthorized();
            }

            if (User.IsInRole("Doctor"))
            {
                var doctor = await doctorService.GetByUserIdAsync(userId);

                var treated = await appointmentService
                    .HasDoctorTreatedPatient(doctor.DoctorId, id);

                if (!treated)
                    return Unauthorized();
            }

            return Ok(await patientService.GetByIdAsync(id));
        }

        [HttpGet("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var patient = await patientService.GetByUserIdAsync(userId);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdatePatient(int id, PatientDto dto)
        {
            if (id != dto.PatientId)
                return BadRequest("Patient ID mismatch.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var patient = await patientService.GetByUserIdAsync(userId);
            if (patient == null || patient.PatientId != id)
                return Unauthorized();

            var updated = await patientService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpGet("appointments")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = await patientService.GetByUserIdAsync(userId);

            return Ok(await appointmentService
                .GetByPatientIdAsync(patient.PatientId));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            return Ok(await patientService.GetAllAsync(1, 10));
        }
    }
}