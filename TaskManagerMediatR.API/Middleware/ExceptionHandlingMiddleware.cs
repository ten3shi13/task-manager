using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerMediatR.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IHostEnvironment env,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server.Error",
                Detail = _env.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred",
                Instance = context.Request.Path
            };

            problem.Extensions["errorCode"] = "Server.UnexpectedError";
            problem.Extensions["traceId"] = context.TraceIdentifier;

            if (_env.IsDevelopment())
            {
                problem.Extensions["exception"] = exception.ToString();
            }

            context.Response.StatusCode = problem.Status.Value;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
