using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Services;
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
        IDoctorService doctorService) : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePatient(CreatePatientDto dto)
        {
            var result = await patientService.AddAsync(dto);
            return Ok(result);
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