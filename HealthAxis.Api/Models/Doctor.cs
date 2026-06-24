using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Z][A-Za-z\s]+$")]
        public required string FullName { get; set; }
        [Required]
        [RegularExpression("(Endocrinologist|Oncologist|Gynecologist|OrthopedicSurgeon|Psychiatrist|Pediatrician|Neurologist|Dermatologist|Cardiologist|GeneralPractitioner)", ErrorMessage = "Invalid Specialisation")]
        public required string Specialisation { get; set; }
        [Required]
        [Range(0, 70, ErrorMessage = "Years of experience should be between o to 70")]

        public int YearsOfExperience { get; set; }
        [Required]
        [Range(0, 100000)]
        public int ConsultationFee { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}


