using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models.Dtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public required string TimeSlot { get; set; }

        [Required]
        [RegularExpression("^(Pending|Confirmed|Cancelled|Completed)$")]
        public required string Status { get; set; }

        [MaxLength(100)]
        public string? CancellationReason { get; set; }
    }
}