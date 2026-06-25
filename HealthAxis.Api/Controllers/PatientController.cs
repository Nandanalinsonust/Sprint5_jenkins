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
            var patient = await patientService.AddAsync(dto);

            var result = await authService.CreatePatientUser(dto.Email);

            if (!result.Success)
                return BadRequest(result.Message);

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
            return Ok(patient);
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