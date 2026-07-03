using HealthAxis.Api.Services;
using HealthAxis.Shared.Dtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController(IDoctorService doctorService) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDoctors()
        {
            return Ok(await doctorService.GetAllAsync(1,10));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var result = await doctorService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("specialisation/{specialisation}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorsBySpecialisation(DoctorSpecialisation specialisation)
        {
            return Ok(await doctorService.GetBySpecialisationAsync(specialisation));
        }


        [HttpGet("name/{name}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorsByName(string name)
        {
            return Ok(await doctorService.GetByNameAsync(name));
        }

        [HttpGet("availability/{doctorId}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetAvailability(int doctorId, DateTime date)
        {
            return Ok(await doctorService.GetAvailabilityAsync(doctorId, date));
        }

        [HttpGet("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var doctor = await doctorService.GetByUserIdAsync(userId);

            if (doctor == null) return NotFound();

            return Ok(doctor);
        }

        [HttpPut("{id}")]
[Authorize(Roles = "Doctor")]
public async Task<IActionResult> Update(
    int id,
    UpdateDoctorDto dto)
{
    var userId =
        User.FindFirstValue(
            ClaimTypes.NameIdentifier);

    var currentDoctor =
        await doctorService
            .GetByUserIdAsync(userId);

    if (currentDoctor.DoctorId != id)
        return Unauthorized();

    return Ok(
        await doctorService.UpdateAsync(
            id,
            dto
        )
    );
}
[HttpPut("change-password")]
[Authorize(Roles = "Doctor")]
public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    var doctor = await doctorService.GetByUserIdAsync(userId);

    if (doctor == null)
        return NotFound();

    var result = await doctorService.ChangePasswordAsync(userId, dto);

    if (!result)
        return BadRequest("Current password is incorrect");

    return Ok(new { message = "Password updated successfully" });
}
    }
}