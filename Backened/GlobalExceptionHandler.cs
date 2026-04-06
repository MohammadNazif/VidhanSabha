using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Api.GlobalExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(IHostEnvironment env)
        {
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ForbiddenException => StatusCodes.Status403Forbidden,
                SqlException => StatusCodes.Status500InternalServerError,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var problem = new ProblemDetails
            {
                Title = GetTitle(statusCode),
                Status = statusCode,
                Instance = context.Request.Path,
                Detail = _env.IsDevelopment()
                    ? exception.Message
                    : "Something went wrong"
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;
            problem.Extensions["timestamp"] = DateTime.UtcNow;

            // Use your custom ValidationException for errors
            if (exception is VidhanSabha.Application.Exceptions.ValidationException validation)
            {
                problem.Extensions["errors"] = validation.Errors;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }

        private static string GetTitle(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Server Error",
            _ => "Error"
        };
    }
}