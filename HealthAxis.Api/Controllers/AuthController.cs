using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var result = await authService.Register(request);

            if (!result.Success)
            {
                return BadRequest(new {message = result.Message});
            }

            return Ok(new
            {
                message = result.Message,
                userId = result.UserId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await authService.Login(request);

            if (!result.Success)
            {
                return Unauthorized(new {message = result.Message});
            }

            return Ok(new
            {
                message = result.Message,
                token = result.token,
                expiry = result.ExpiresIn
            });
        }
    }
}