namespace HealthAxis.Api.Messaging.Contracts
{
    public record AppointmentBookedEvent
    {
        public int AppointmentId { get; init; }
        public int DoctorId { get; init; }
        public string DoctorName { get; init; } = string.Empty;
        public string PatientName { get; init; } = string.Empty;
        public DateTime ScheduledDate { get; init; }
        public string TimeSlot { get; init; } = string.Empty;
    }
}