using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [Required]
        [RegularExpression(@"[A-Z][A-za-z\s]+", ErrorMessage = "Name should contain only Alphabets")]
        [MinLength(2)]
        public required string FullName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [RegularExpression("(Male|Female|Transgender|Other)", ErrorMessage = "Gender should be Male , Female , Transgender or other")]
        public required string Gender { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public required string Email { get; set; }
        [Required]
        [Phone(ErrorMessage = "Phone Number is invalid")]
        public required string PhoneNumber { get; set; }
        [RegularExpression(@"^$|^INS\d{4}$", ErrorMessage = "Format must be INSXXXX (4 digits)")]
        public string? InsuranceID { get; set; }
        public bool IsActive { get; set; }
    }
}