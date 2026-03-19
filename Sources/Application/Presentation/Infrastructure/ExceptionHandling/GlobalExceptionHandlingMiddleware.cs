using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using JassApp.Common.Logging.Services;
using JetBrains.Annotations;

namespace JassApp.Presentation.Infrastructure.ExceptionHandling
{
    [PublicAPI]
    [SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Microsoft Interface")]
    public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                loggingService.LogException(exception);
                var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred while processing the request." });
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}