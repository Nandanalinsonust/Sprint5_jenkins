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

    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public bool IsFirstLogin { get; set; }
        public string Role { get; set; }
    }

    public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
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