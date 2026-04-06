
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VidhanSabha.Application.Exceptions;


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
            ArgumentException => StatusCodes.Status400BadRequest,
            SqlException => StatusCodes.Status500InternalServerError,
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

        if (exception is ValidationException validation)
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