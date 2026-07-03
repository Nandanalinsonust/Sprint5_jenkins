using System.ComponentModel.DataAnnotations;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Shared.Dtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; }

        public DoctorSpecialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateDoctorDto
    {
        

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Z][A-Za-z\s]+$")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DoctorSpecialisation Specialisation { get; set; }

        [Required]
        [Range(0, 70)]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000)]
        public int ConsultationFee { get; set; }
    }

    public class UpdateDoctorDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        [MinLength(2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DoctorSpecialisation Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
    public class DoctorChartDto
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public int Count { get; set; }
    }
}
