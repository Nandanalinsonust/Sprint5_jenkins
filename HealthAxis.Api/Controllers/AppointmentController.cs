using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Patient")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.AddAsync(entity);

            return CreatedAtAction(nameof(GetAppointmentById),
                new { id = result.AppointmentId },
                result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var result = await service.GetByIdAsync(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
        {
            var result = await service.GetByDoctorIdAsync(doctorId);
            return Ok(result);
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
        {
            var result = await service.GetByPatientIdAsync(patientId);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Patient,Doctor")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] UpdateAppointmentStatusDto dto)
        {
            var result = await service.UpdateStatusAsync(id, dto.Status);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await service.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}