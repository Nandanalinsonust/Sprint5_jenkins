using Garnet;

namespace HealthAxis.Api.Services.Impl
{
    public class GarnetHostedService : IHostedService, IDisposable
    {
        private GarnetServer? server;
        private bool disposed;

        private readonly ILogger<GarnetHostedService> logger;

        public GarnetHostedService(ILogger<GarnetHostedService> logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                server = new GarnetServer(["--port=6379"]);

                server.Start();

                logger.LogInformation("Embedded Garnet server started on port 6379.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to start embedded Garnet server.");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();

            logger.LogInformation("Embedded Garnet server stopped.");

            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                server?.Dispose();
                server = null;
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}