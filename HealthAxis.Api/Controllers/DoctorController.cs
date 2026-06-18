using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController(IDoctorService doctorservice) : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await doctorservice.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var result = await doctorservice.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("name/{name}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorsByName(string name)
        {
            var result = await doctorservice.GetByNameAsync(name);
            return Ok(result);
        }

        [HttpGet("specialisation/{specialisation}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorsBySpecialisation(string specialisation)
        {
            var result = await doctorservice.GetBySpecialisationAsync(specialisation);
            return Ok(result);
        }

        [HttpGet("availability/{doctorId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorAvailability(int doctorId, [FromQuery] DateTime date)
        {
            var result = await doctorservice.GetAvailabilityAsync(doctorId, date);
            return Ok(result);
        }
    }
}