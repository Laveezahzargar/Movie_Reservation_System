
using P12_Movie_Reservation_System_Backend.Common;

namespace P12_Movie_Reservation_System_Backend.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. TraceId: {TraceId}, Method: {Method}, Path: {Path}",
                    context.TraceIdentifier,
                    context.Request.Method,
                    context.Request.Path);

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started.");
                    throw;
                }

                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = ApiResponse<object>.FailureResponse(
                    "An unexpected error occurred. Please try again later.");

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}