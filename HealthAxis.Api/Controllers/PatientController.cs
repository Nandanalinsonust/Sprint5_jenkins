using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController(IPatientService patientService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await patientService.AddAsync(dto);
            return CreatedAtAction("GetPatientById", new { id = result.PatientId }, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var result = await patientService.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await patientService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("name/{name}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
        public async Task<IActionResult> GetPatientsByName(string name)
        {
            var result = await patientService.GetByNameAsync(name);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Admin")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await patientService.UpdateAsync(id, dto);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut("deactivate/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeactivatePatient(int id)
        {
            var result = await patientService.DeactivateAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}