using HealthAxis.Api.Exceptions;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using HealthAxis.Shared.Dtos.Pagination;
using HealthAxis.Shared.Dtos.Auth;


namespace HealthAxis.Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception,
                "An unexpected error occurred: {Message}",
                exception.Message);

            var (statusCode, message) = exception switch
            {
                EntityNotFoundException ex =>
                    (StatusCodes.Status404NotFound, ex.Message),

                ConflictException ex =>
                    (StatusCodes.Status409Conflict, ex.Message),

                AppointmentRuleException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),

                HealthRecordRuleException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),
                ForbiddenAccessException ex =>
                    (StatusCodes.Status403Forbidden, ex.Message),
                BusinessRuleException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),

                HealthcareAppException ex =>
                    (StatusCodes.Status400BadRequest, ex.Message),

                _ =>
                    (StatusCodes.Status500InternalServerError, "Something went wrong. Please try again later.")
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                Path = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}