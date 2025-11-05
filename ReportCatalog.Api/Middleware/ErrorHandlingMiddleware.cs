using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ReportCatalog.Domain.Errors;

namespace ReportCatalog.Api.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception during request {Path}", context.Request.Path);

        var status = MapStatusCode(ex);
        var title = MapTitle(ex);

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = ex.Message,
            Type = $"https://httpstatuses.io/{(int)status}",
            Instance = context.Request.Path
        };

        if (_env.IsDevelopment())
            problem.Extensions["stackTrace"] = ex.StackTrace;

        problem.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.StatusCode = problem.Status.Value;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _env.IsDevelopment()
        });

        await context.Response.WriteAsync(json);
    }

    private static HttpStatusCode MapStatusCode(Exception ex) => ex switch
    {
        UnsupportedFormatException => HttpStatusCode.BadRequest,
        ArgumentException => HttpStatusCode.BadRequest,
        UnauthorizedAccessException => HttpStatusCode.Unauthorized,
        InvalidOperationException => HttpStatusCode.Conflict,
        KeyNotFoundException => HttpStatusCode.NotFound,
        _ => HttpStatusCode.InternalServerError
    };

    private static string MapTitle(Exception ex) => ex switch
    {
        UnsupportedFormatException => "Unsupported report format",
        ArgumentException => "Invalid argument",
        UnauthorizedAccessException => "Unauthorized access",
        InvalidOperationException => "Invalid operation",
        KeyNotFoundException => "Resource not found",
        _ => "Internal server error"
    };
}
