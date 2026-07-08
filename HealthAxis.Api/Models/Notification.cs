using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public int DoctorId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; }
    }
}