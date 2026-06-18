using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models.Dtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? CancellationReason { get; set; }
    }

    public class CreateAppointmentDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}(-\d{2}:\d{2})?$")]
        public string TimeSlot { get; set; } = string.Empty;
    }


    public class UpdateAppointmentStatusDto
    {
        [Required]
        [RegularExpression("^(Pending|Confirmed|Cancelled|Completed)$")]
        public string Status { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CancellationReason { get; set; }
    }
}