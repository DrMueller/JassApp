using JetBrains.Annotations;

namespace JassApp.Common.Logging.Services
{
    [PublicAPI]
    public interface ILoggingService
    {
        void LogError(string errorMessage);
        void LogException(Exception ex);
        void LogWarning(string warningMessage);
        void TrackEvent(string eventInfo);
    }
}