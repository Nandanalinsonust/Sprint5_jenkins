using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HealthRecordController(IHealthRecordService service) : ControllerBase
    {
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Doctor")]
        public async Task<IActionResult> CreateHealthRecord([FromBody] CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await service.AddAsync(dto);
            return CreatedAtAction(nameof(GetHealthRecordById), new { id = result.HealthRecordId }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHealthRecordById(int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetRecordsByPatientId(int patientId)
        {
            var result = await service.GetByPatientIdAsync(patientId);
            return Ok(result);
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetRecordsByDoctorId(int doctorId)
        {
            var result = await service.GetByDoctorIdAsync(doctorId);
            return Ok(result);
        }
    }
}
