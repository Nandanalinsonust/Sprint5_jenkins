using System.ComponentModel.DataAnnotations;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Shared.Dtos
{
    // ✅ MAIN DTO (READ)
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; }   // ✅ ENUM

        public string? CancellationReason { get; set; }
    }

    // ✅ CREATE DTO
    public class CreateAppointmentDto
    {
        // ❌ Removed AppointmentId (IMPORTANT FIX)

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}(-\d{2}:\d{2})?$",
            ErrorMessage = "Invalid time slot format (HH:mm or HH:mm-HH:mm)")]
        public string TimeSlot { get; set; } = string.Empty;
    }

    // ✅ UPDATE DTO
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

    // ✅ UPDATE STATUS DTO
    public class UpdateAppointmentStatusDto
    {
        [Required]
        public AppointmentStatus Status { get; set; }   // ✅ ENUM

        [MaxLength(100)]
        public string? CancellationReason { get; set; }
    }

    // ✅ REPORT DTO
    public class AppointmentSummaryDto
    {
        public DateTime Date { get; set; }

        public int Pending { get; set; }

        public int Confirmed { get; set; }

        public int Completed { get; set; }

        public int Cancelled { get; set; }
    }
}