using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;

    public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            LogErrorToFile(ex);
            throw; 
        }
    }

    private void LogErrorToFile(Exception exception)
    {
        try
        {
            string logFilePath = "errorlog.txt"; 

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"Timestamp: {DateTime.UtcNow}");
                writer.WriteLine($"Exception Message: {exception.Message}");
                writer.WriteLine($"Stack Trace: {exception.StackTrace}");
                writer.WriteLine(new string('-', 50));
            }

            _logger.LogError($"Error logged to file: {logFilePath}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error logging to file: {ex.Message}");
        }
    }
}
