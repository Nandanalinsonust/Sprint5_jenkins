using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? InsuranceID { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreatePatientDto
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Z][A-Za-z\s]+$")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Transgender|Other)$")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [RegularExpression(@"^$|^INS\d{4}$")]
        public string? InsuranceID { get; set; }
    }
}