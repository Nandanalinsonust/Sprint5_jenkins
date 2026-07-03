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

        // ✅ CREATE APPOINTMENT
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
                return Unauthorized();

            var patient = await patientService.GetByUserIdAsync(userId);

            if (patient == null)
                return BadRequest("Patient not found");

            dto.PatientId = patient.PatientId;

            return Ok(await service.AddAsync(dto));
        }

        // ✅ GET MY APPOINTMENTS
        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMy()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
                return Unauthorized();

            var patient = await patientService.GetByUserIdAsync(userId);

            return Ok(await service.GetByPatientIdAsync(patient.PatientId));
        }

        // ✅ DOCTOR APPOINTMENTS
        [HttpGet("doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
                return Unauthorized();

            var doctor = await doctorService.GetByUserIdAsync(userId);

            return Ok(await service.GetByDoctorIdAsync(doctor.DoctorId));
        }

        // ✅ ADMIN - ALL APPOINTMENTS
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await service.GetAllAsync());
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var appt = await service.GetByIdAsync(id);

            if (appt == null)
                return NotFound();

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

        // ✅ DELETE APPOINTMENT
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.CancelWithValidation(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // ✅ UPDATE STATUS
        [HttpPut("status/{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateAppointmentStatusDto dto)
        {
            var result = await service.UpdateStatusAsync(id, dto.Status);

            return Ok(result);
        }
    }
}