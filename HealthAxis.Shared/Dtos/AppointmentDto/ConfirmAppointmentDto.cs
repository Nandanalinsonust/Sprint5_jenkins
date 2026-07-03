using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos.Appointments    
{
    public class ConfirmAppointmentDto
    {
        [Required(ErrorMessage = "Please select the doctor confirming this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid doctor reference.")]
        public int DoctorId { get; set; }
    }
}