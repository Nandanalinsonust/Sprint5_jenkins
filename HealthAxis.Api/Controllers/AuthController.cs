using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.Register(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                userId = result.UserId
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.Login(request);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.Data,
                expiresIn = result.ExpiresIn
            });
        }
    }
}