using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Z][A-Za-z\s]+$", ErrorMessage = "Name should contain only alphabets")]
        public required string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Transgender|Other)$",
            ErrorMessage = "Gender must be valid")]
        public required string Gender { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public required string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public required string PhoneNumber { get; set; }

        [RegularExpression(@"^$|^INS\d{4}$",
            ErrorMessage = "Format must be INSXXXX")]
        public string? InsuranceID { get; set; }

        public bool IsActive { get; set; }
    }
}