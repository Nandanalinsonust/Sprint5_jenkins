using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models.Dtos
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Patient";
    }
}
