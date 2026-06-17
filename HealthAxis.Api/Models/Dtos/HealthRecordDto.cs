using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models.Dtos
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        public DateTime VisitDate { get; set; }

        [Required]
        public string Diagnosis { get; set; } = string.Empty;

        [Required]
        public string Prescription { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}