using HealthAxis.Api.Models;
using HealthAxis.Shared.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
    public class AdminController(IDoctorService doctorService, IPatientService patientService, IAppointmentService appointmentService,IAuthService authService, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor(CreateDoctorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var doctor = await doctorService.AddAsync(dto);

            var userResult = await authService.CreateDoctorUser(dto.Email);

            if (!userResult.Success)
                return BadRequest(userResult.Message);

            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return BadRequest("User creation failed");

            await doctorService.AssignUserAsync(doctor.DoctorId, user.Id);

            return Ok(doctor);
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

        [HttpGet("doctors/list")]
        public async Task<IActionResult> GetAllDoctors(int page = 1, int pageSize = 10)
        {
            var result = await doctorService.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("patients/list")]
        public async Task<IActionResult> GetAllPatients(int page = 1, int pageSize = 10)
        {
            var result = await patientService.GetAllAsync(page, pageSize);
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