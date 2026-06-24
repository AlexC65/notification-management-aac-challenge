using System.Net;
using System.Text.Json;
using NotificationManagement.Domain.Exceptions;


namespace NotificationManagement.API.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (DomainException ex)
        {
            // Expected — warn only
            _logger.LogWarning(ex, "Domain exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message) = ex switch
        {
            //Domain exceptions
            InvalidPasswordException => (HttpStatusCode.Unauthorized, ex.Message),        // 401
            UserNotFoundException => (HttpStatusCode.NotFound, ex.Message),        // 404
            InvalidEmailException     => (HttpStatusCode.BadRequest,   ex.Message),  // 400
            DomainException => (HttpStatusCode.BadRequest, ex.Message),        // 400

            //Built-in exceptions
            KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),        // 404
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, ex.Message),        // 403
            InvalidOperationException => (HttpStatusCode.Conflict, ex.Message),        // 409

            //Catch-all
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")    // 500
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var body = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(body);
    }
}
