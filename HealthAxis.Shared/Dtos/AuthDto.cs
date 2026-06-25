using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Patient";
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;

    }

    public class ChangePasswordDto
    {
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Message { get; set; } = string.Empty;

        public AuthResponse Data { get; set; } = new();

        public int ExpiresIn { get; set; }
    }
}