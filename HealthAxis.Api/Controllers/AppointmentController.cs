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
    public class AppointmentController(
        IAppointmentService service,
        IPatientService patientService,
        IDoctorService doctorService) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = await patientService.GetByUserIdAsync(userId!);

            dto.PatientId = patient.PatientId;

            return Ok(await service.AddAsync(dto));
        }

        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMy()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var patient = await patientService.GetByUserIdAsync(userId!);

            return Ok(await service.GetByPatientIdAsync(patient.PatientId));
        }

        [HttpGet("doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var doctor = await doctorService.GetByUserIdAsync(userId!);

            return Ok(await service.GetByDoctorIdAsync(doctor.DoctorId));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appt = await service.GetByIdAsync(id);

            if (appt == null) return NotFound();

            if (User.IsInRole("Patient"))
            {
                var patient = await patientService.GetByUserIdAsync(userId!);

                if (appt.PatientId != patient.PatientId)
                    return Unauthorized();
            }

            if (User.IsInRole("Doctor"))
            {
                var doctor = await doctorService.GetByUserIdAsync(userId!);

                if (appt.DoctorId != doctor.DoctorId)
                    return Unauthorized();
            }

            return Ok(appt);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.CancelWithValidation(id);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}