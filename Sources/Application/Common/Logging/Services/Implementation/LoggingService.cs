using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.ApplicationInsights;

namespace JassApp.Common.Logging.Services.Implementation
{
    [UsedImplicitly]
    public class LoggingService(
        TelemetryClient telemetryClient,
        ILogger<LoggingService> loggingService)
        : ILoggingService
    {
        private readonly bool _islocalEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public void LogError(string errorMessage)
        {
            LogLocal(errorMessage);
            loggingService.LogError(errorMessage);
        }

        public void LogException(Exception ex)
        {
            LogLocal(ex.Message);
            loggingService.LogError(ex, ex.Message);
        }

        public void LogWarning(string warningMessage)
        {
            LogLocal(warningMessage);
            loggingService.LogWarning(warningMessage);
        }

        public void TrackEvent(string eventInfo)
        {
            telemetryClient.TrackEvent(eventInfo);
            LogLocal(eventInfo);
        }

        private void LogLocal(string message)
        {
            if (_islocalEnvironment)
            {
                Debug.WriteLine(message);
            }
        }
    }
}