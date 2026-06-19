using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using HealthAxis.Api.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
    public class AdminController(IDoctorService doctorService, IPatientService patientService, IAppointmentService appointmentService,IAuthService authService) : ControllerBase
    {
        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await doctorService.AddAsync(dto);

            await authService.CreateDoctorUser(dto.Email);

            return CreatedAtAction("GetAllDoctors", new { id = result.DoctorId }, result);
        }


        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await doctorService.UpdateAsync(id, dto);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut("doctors/deactivate/{id}")]
        public async Task<IActionResult> DeactivateDoctor(int id)
        {
            var result = await doctorService.DeactivateAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await doctorService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("patients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await patientService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("patients/deactivate/{id}")]
        public async Task<IActionResult> DeactivatePatient(int id)
        {
            var result = await patientService.DeactivateAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentSummaryReports()
        {
            var result = await appointmentService.GetSummaryReportAsync();
            return Ok(result);
        }
    }
}