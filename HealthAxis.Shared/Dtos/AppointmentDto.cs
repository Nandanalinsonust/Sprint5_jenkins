using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
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
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}(-\d{2}:\d{2})?$")]
        public string TimeSlot { get; set; } = string.Empty;
    }

    public class UpdateAppointmentDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
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
    //public class AppointmentReportDto
    //{
    //    public DateTime Date { get; set; }
    //    public int Confirmed { get; set; }
    //    public int Cancelled { get; set; }
    //    public int Completed { get; set; }
    //}
    public class AppointmentSummaryDto
    {
        public DateTime Date { get; set; }
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}