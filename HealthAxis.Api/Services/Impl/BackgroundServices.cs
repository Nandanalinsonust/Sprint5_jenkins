using Microsoft.Extensions.Hosting;

namespace HealthAxis.Api.BackgroundServices
{
    public class HeartbeatBackgroundService : BackgroundService
    {
        private readonly ILogger<HeartbeatBackgroundService> logger;

        private static readonly Action<ILogger, DateTime, Exception?> HeartbeatRunning =
            LoggerMessage.Define<DateTime>(
                LogLevel.Information,
                new EventId(1, nameof(HeartbeatRunning)),
                "Heartbeat running at: {Time}");

        public HeartbeatBackgroundService(
            ILogger<HeartbeatBackgroundService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                HeartbeatRunning(
                    logger,
                    DateTime.UtcNow,
                    null);

                await Task.Delay(
                    TimeSpan.FromSeconds(10),
                    stoppingToken);
            }
        }
    }
}