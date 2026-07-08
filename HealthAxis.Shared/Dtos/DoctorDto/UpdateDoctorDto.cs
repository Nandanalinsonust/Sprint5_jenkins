using System;
using System.ComponentModel.DataAnnotations;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Shared.Dtos.Doctors
{
    public class UpdateDoctorDto
    {
        [Required(ErrorMessage = "Please enter the doctor's full name.")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "Doctor name must be between 2 and 100 characters.")]
        [RegularExpression(
            @"^[A-Za-z]+(?: [A-Za-z]+)*$",
            ErrorMessage = "Doctor name can contain only letters and single spaces between words.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select the doctor's specialisation.")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Please enter years of experience.")]
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Please enter the consultation fee.")]
        [Range(
            1,
            100000,
            ErrorMessage = "Consultation fee must be between 1 and 100,000.")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}