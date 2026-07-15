using HealthAxis.Api.Data;
using HealthAxis.Api.Messaging.Contracts;
using HealthAxis.Api.Models;
using MassTransit;

namespace HealthAxis.Api.Messaging.Consumers
{
    public class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
    {
        private readonly HealthAxisDbContext _dbContext;
        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(HealthAxisDbContext dbContext,ILogger<AppointmentBookedConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
        {
            var message = context.Message;

            var notification = new Notification
            {
                DoctorId = message.DoctorId,
                Message =
                    $"New appointment booked by {message.PatientName} on {message.ScheduledDate:dd-MM-yyyy} at {message.TimeSlot}",
                CreatedDate = DateTime.UtcNow,
                IsRead = false
            };

            _dbContext.Notifications.Add(notification);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Notification created for Doctor {DoctorId}",
                message.DoctorId);

        }
    }
}